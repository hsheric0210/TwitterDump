using System.Diagnostics;
using System.Text;

namespace TwitterDump
{
	public static class Program
	{
		public static readonly string[] LineSeparators = new string[3] { "\r\n", "\r", "\n" };

		public static void Main(string[] args) => MainAsync(args).Wait();

		public static async Task MainAsync(string[] args)
		{
			var configFileName = (args.Length > 0) ? args[0] : "TwitterDump.ini";
			if (!File.Exists(configFileName))
				Config.SaveDefaults(configFileName);

			Console.WriteLine($"Reading configuration file: {configFileName}");
			var config = new Config(new IniFile(configFileName));

			string inputFileName = config.InputFileName;
			if (CheckFileExistence(config.GalleryDLExecutable, "Gallery-DL executable") || CheckFileExistence(config.Aria2Executable, "Aria2 Executable") || CheckFileExistence(inputFileName, "Member ID list"))
				return;

			Console.WriteLine($"Reading input file: {inputFileName}");
			string[] _targets = File.ReadAllLines(inputFileName);
			IEnumerable<Target> targets = from target in _targets where !string.IsNullOrWhiteSpace(target) select new Target(target);

			var extractorAsDownloader = config.ExtractorAsDownloader;

			var tasks = new List<Task>();
			foreach (Target target in targets)
			{
				tasks.Add(Task.Run(async () =>
				{
					if (extractorAsDownloader)
						Console.WriteLine($"Now downloading: '{target.ID}'");
					else
						Console.WriteLine($"Now retrieving: '{target.ID}'");
					var extractionResult = await Extract(config, target);

					if (!extractorAsDownloader)
					{
						Console.WriteLine($"Successfully retrieved: '{target.ID}'");

						Console.WriteLine($"Now building aria2 input file: '{target.ID}'.");
						List<string> aria2InputLines = MakeAria2InputFile(target, extractionResult);
						Console.WriteLine($"Successfully built aria2 input file: '{target.ID}'.");

						Console.WriteLine($"Now downloading: '{target.ID}'");
						await Download(config, target, aria2InputLines);
					}

					Console.WriteLine($"Successfully downloaded: '{target.ID}'");
				}));
			}

			while (tasks.Count > 0)
				tasks.Remove(await Task.WhenAny(tasks));

			Console.WriteLine("Finished all jobs. Exiting...");
		}

		private static List<string> MakeAria2InputFile(Target target, string extractionResult)
		{
			string[] extractedURLs = extractionResult.Split(LineSeparators, StringSplitOptions.TrimEntries);
			int urlCount = extractedURLs.Length;
			var mainURLs = new List<(string, string?)>(urlCount);
			var mirrorURLs = new Dictionary<string, List<string>>();
			string? lastMainURL = null;
			var duplicateCheckSet = new HashSet<string>();
			foreach (string url in extractedURLs)
			{
				if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
				{
					lastMainURL = url.Trim();
					string? newFileName = target.protocol.NewFileNameRetriever(url);
					string fileName = newFileName ?? url.ExtractFileName();

					if (duplicateCheckSet.Contains(fileName))
					{
						Console.WriteLine($"Skipped '{fileName}' because it's already processed.");
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

		private static async Task<string?> Extract(Config config, Target target)
		{
			var gallery_dl = new Process();
			gallery_dl.StartInfo.FileName = config.GalleryDLExecutable;
			gallery_dl.StartInfo.Arguments = config.GetGalleryDLParameter(target.ID);
			gallery_dl.StartInfo.UseShellExecute = false;
			gallery_dl.StartInfo.RedirectStandardOutput = true;

			gallery_dl.Start();

			if (config.ExtractorAsDownloader)
			{
				using var stream = new MemoryStream();
				await gallery_dl.StandardOutput.BaseStream.CopyToAsync(stream, 4096);
				await gallery_dl.WaitForExitAsync();
				return Encoding.UTF8.GetString(stream.ToArray());
			}
			else
			{
				await gallery_dl.WaitForExitAsync();
				return null;
			}
		}

		private static async Task Download(Config config, Target target, List<string> input)
		{
			// Workaround
			string tmpFileName = $"{target.ID.ToFileName()}.{Random.Shared.NextInt64()}";
			await File.WriteAllLinesAsync(tmpFileName, input);

			var aria2 = new Process();
			aria2.StartInfo.FileName = config.Aria2Executable;
			aria2.StartInfo.Arguments = config.GetAria2Parameter(target.ID, tmpFileName);
			aria2.StartInfo.UseShellExecute = true;
			aria2.Start();
			await aria2.WaitForExitAsync();
			File.Delete(tmpFileName);
		}

		private static bool CheckFileExistence(string fileName, string description)
		{
			if (!File.Exists(fileName))
			{
				PrintError($"File {fileName}({description}) not found");
				return true;
			}
			return false;
		}

		private static void PrintError(string message)
		{
			ConsoleColor prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ForegroundColor = prevColor;
		}
	}
}
