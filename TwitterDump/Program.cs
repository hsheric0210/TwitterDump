﻿using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Events;

namespace TwitterDump
{
	public static class Program
	{
		public static readonly string[] LineSeparators = new string[3] { "\r\n", "\r", "\n" };
		private static readonly JsonSerializerOptions JsonOptions = new()
		{
			WriteIndented = true
		};

		public static void Main(string[] args) => MainAsync(args).Wait();

		public static async Task MainAsync(string[] args)
		{
			Config config = new Config();

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Async(opt => opt.File(args.GetCmdSwitch("l", "TwitterDump.log"), buffered: true, flushToDiskInterval: TimeSpan.FromMilliseconds(100), fileSizeLimitBytes: 16777216 /* 128MB */, rollOnFileSizeLimit: true), bufferSize: 8192)
				.WriteTo.Async(opt => opt.Console(restrictedToMinimumLevel: LogEventLevel.Information, theme: AnsiConsoleTheme.Code), bufferSize: 16384)
				.CreateLogger();

			try
			{
				var configFileName = args.GetCmdSwitch("c", "TwitterDump.json");
				if (!File.Exists(configFileName) || args.HasCmdSwitch("gc"))
				{
					try
					{
						Log.Information("Exporting default config file to {file}.", configFileName);
						await File.WriteAllTextAsync(configFileName, JsonSerializer.Serialize(config, JsonOptions));
					}
					catch (Exception e)
					{
						Log.Error(e, "An exception occurred during exporting default config file.");
					}

					await Log.CloseAndFlushAsync();
					return;
				}

				Log.Information("Using configuration file {file}.", configFileName);
				try
				{
					string data = await File.ReadAllTextAsync(configFileName);
					Config? parsed = JsonSerializer.Deserialize<Config>(data, JsonOptions);
					if (parsed == null)
						Log.Warning("Json deserializer returned null! Using default config instead.");
					else
						config = parsed;
				}
				catch (Exception e)
				{
					Log.Error(e, "An exception occurred during config parse.");
				}

				string inputFileName = config.ListFile;
				if (CheckFileExistence(config.Retriever.Executable, "Gallery-DL executable") || CheckFileExistence(config.Downloader.Executable, "Aria2 Executable") || CheckFileExistence(inputFileName, "Member ID list"))
				{
					await Log.CloseAndFlushAsync();
					return;
				}

				Log.Information("Reading list file {file}.", inputFileName);
				string[] _targets = File.ReadAllLines(inputFileName);
				IEnumerable<Target> targets = from target in _targets where !string.IsNullOrWhiteSpace(target) select new Target(target);

				var useRetrieverAsDownloader = config.UseRetrieverIntegratedDownloader;

				var tasks = new List<Task>();
				using var retrieverParallellismLimiter = new SemaphoreSlim(config.Parallelism.RetrieverParallelism);
				using var downloaderParallellismLimiter = new SemaphoreSlim(config.Parallelism.DownloaderParallelism);
				foreach (Target target in targets)
				{
					tasks.Add(Task.Run(async () =>
					{
						List<string>? downloadListLines = await RetrieveTask(config, target, retrieverParallellismLimiter, useRetrieverAsDownloader);
						if (downloadListLines != null && downloadListLines.Count > 0)
							await DownloadTask(config, target, downloadListLines, downloaderParallellismLimiter);
						else
							Log.Warning("Skipped member {member} as there's no updates.", target.ID);
					}));
				}

				await Task.WhenAll(tasks);

				Log.Information("Finished all jobs. Exiting...");
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Exception caught in main thread!");
			}
			finally
			{
				await Log.CloseAndFlushAsync();
			}
		}

		private static async Task DownloadTask(Config config, Target target, List<string> aria2InputLines, SemaphoreSlim downloaderParallellismLimiter)
		{
			Log.Debug("Waiting for downloader parallellism semaphore...");
			await downloaderParallellismLimiter.WaitAsync();

			// Enqueue download task
			Log.Information("Now downloading: {id}.", target.ID);
			try
			{
				await Download(config, target, aria2InputLines);
				Log.Information("Successfully downloaded: {id}.", target.ID);
			}
			finally
			{
				downloaderParallellismLimiter.Release();
			}
		}

		private static async Task<List<string>?> RetrieveTask(Config config, Target target, SemaphoreSlim semaphore, bool useRetrieverAsDownloader)
		{
			await semaphore.WaitAsync();

			try
			{
				Log.Information("Now retrieving: {id}", target.ID);

				// Retrieve media CDN URL.
				var retrievedResult = await Retrieve(config, target);

				if (!useRetrieverAsDownloader)
				{
					Log.Information("Successfully retrieved: {id}.", target.ID);

					Log.Information("Building download list: {id}.", target.ID);
					List<string> result = MakeDownloadList(config, target, retrievedResult!);
					Log.Information("Successfully built download list: {id}.", target.ID);
					return result;
				}
			}
			finally
			{
				semaphore.Release();
			}

			return null;
		}

		private static List<string> MakeDownloadList(Config config, Target target, string retrievedResult)
		{
			string[] retrievedURLs = retrievedResult.Split(LineSeparators, StringSplitOptions.TrimEntries);
			int urlCount = retrievedURLs.Length;
			var mainURLs = new List<(string, string?)>(urlCount);
			var mirrorURLs = new Dictionary<string, List<string>>();
			string? lastMainURL = null;
			var duplicateCheckSet = new HashSet<string>();
			foreach (string url in retrievedURLs)
			{
				if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
				{
					lastMainURL = url.Trim();
					string? newFileName = target.protocol.NewFileNameRetriever(url);
					string fileName = newFileName ?? url.ExtractFileName();

					if (config.SkipAlreadyExists)
					{
						string path = Path.Combine(config.Destination.FormatDestination(target.ID), fileName);
						if (new FileInfo(path).Exists)
						{
							Log.Debug("File {file} already exists! Skipping media {media} of member {member}.", path, fileName, target.ID);
							continue;
						}
					}

					if (duplicateCheckSet.Contains(fileName))
					{
						Log.Debug("Skipped {file} as it's already processed previously.", fileName);
						continue;
					}
					else
					{
						mainURLs.Add((lastMainURL, newFileName));
					}

					duplicateCheckSet.Add(fileName);
				}
				else if (lastMainURL != null && url.StartsWith('|'))
				{
					if (!mirrorURLs.ContainsKey(lastMainURL))
						mirrorURLs[lastMainURL] = new List<string>();
					mirrorURLs[lastMainURL].Add(url[1..].Trim());
				}
			}

			var aria2InputLines = new List<string>(urlCount);
			foreach ((string mainURL, string? newFileName) in mainURLs)
			{
				var builder = new StringBuilder();
				builder.Append(mainURL);
				if (mirrorURLs.ContainsKey(mainURL))
				{
					builder.Append('\t');
					builder.AppendJoin('\t', mirrorURLs[mainURL]);
				}
				aria2InputLines.Add(builder.ToString());
				if (newFileName != null)
					aria2InputLines.Add($"  out={newFileName}");
			}

			return aria2InputLines;
		}

		private static async Task<string?> Retrieve(Config config, Target target)
		{
			var retriever = new Process();
			retriever.StartInfo.FileName = config.Retriever.Executable;
			retriever.StartInfo.Arguments = config.Retriever.Parameters.FormatRetriverParameters(target.ID);
			Log.Debug("Running retriever with argument {args}.", retriever.StartInfo.Arguments);
			retriever.StartInfo.UseShellExecute = false;
			retriever.StartInfo.RedirectStandardOutput = true;

			retriever.Start();

			if (config.UseRetrieverIntegratedDownloader)
			{
				await retriever.WaitForExitAsync();
				return null;
			}
			else
			{
				using var stream = new MemoryStream();
				await retriever.StandardOutput.BaseStream.CopyToAsync(stream, 4096);
				await retriever.WaitForExitAsync();
				return Encoding.UTF8.GetString(stream.ToArray());
			}
		}

		private static async Task Download(Config config, Target target, List<string> input)
		{
			// Workaround
			string tmpFileName = $"_dl_list_{target.ID}_{Path.GetRandomFileName()}";
			await File.WriteAllLinesAsync(tmpFileName, input);

			var downloader = new Process();
			downloader.StartInfo.FileName = config.Downloader.Executable;
			downloader.StartInfo.Arguments = config.Downloader.Parameters.FormatDownloaderParameters(target.ID, tmpFileName, config.Destination);
			Log.Debug("Running downloader with argument '{args}'.", downloader.StartInfo.Arguments);
			downloader.StartInfo.UseShellExecute = true;
			downloader.Start();
			await downloader.WaitForExitAsync();
			if (!config.KeepDownloadListFile)
				File.Delete(tmpFileName);
		}

		private static bool CheckFileExistence(string fileName, string description)
		{
			if (!File.Exists(fileName))
			{
				Log.Fatal("File {file} ({desc}) not found! The program will exit.", fileName, description);
				return true;
			}
			return false;
		}
	}
}
