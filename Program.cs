
using System.Collections.Concurrent;
using System.Diagnostics;

namespace TwitterDump
{
	class Program
	{
		public static void Main(string[] args)
		{
			var configFileName = (args.Length > 0) ? args[0] : "TwitterDump.ini";
			if (!File.Exists(configFileName))
				Config.saveDefaults(configFileName);
			var config = new Config(new IniFile(configFileName));


			if (checkFileExistence(config.galleryDLExecutable, "Gallery-DL executable") || checkFileExistence(config.aria2Executable, "Aria2 Executable") || checkFileExistence(config.listFileName, "Member ID list"))
				return;

			string[] members = File.ReadAllLines(config.listFileName);
			if (members.Any(member => member.Contains("twitter.com")))
			{
				printError($"Member list({config.listFileName}) contains Twitter URL! Member list must be configured with only member IDs");
				return;
			}

			if (!Directory.Exists(config.imageListFolder))
				Directory.CreateDirectory(config.imageListFolder);

			var memberURLs = new List<string>(members.Length);

			Console.WriteLine("Retrieving member image list.");

			Parallel.ForEach(members, member =>
			{
				var memberURLFile = config.getImageListFilePath(member);
				if (!File.Exists(memberURLFile))
				{
					var gallery_dl = new Process();
					gallery_dl.StartInfo.FileName = config.galleryDLExecutable;
					gallery_dl.StartInfo.Arguments = config.getGalleryDLParameter(member);
					gallery_dl.StartInfo.UseShellExecute = false;
					gallery_dl.StartInfo.RedirectStandardOutput = true;

					gallery_dl.Start();

					using (Stream fileStream = File.OpenWrite(memberURLFile), bufferedStream = new BufferedStream(fileStream))
						gallery_dl.StandardOutput.BaseStream.CopyTo(bufferedStream, 4096);

					gallery_dl.WaitForExit();

					Console.WriteLine($"Extracted image list for {member}.");
				}
			});

			Console.WriteLine("Finished extracting image list.");

			var imageExtension = new ConcurrentDictionary<string, string>();

			var extensionFileName = config.extensionDictionaryFileName;
			bool extensionFileExists = File.Exists(extensionFileName);
			if (extensionFileExists)
				foreach (string line in File.ReadAllLines(extensionFileName))
				{
					int sep = line.IndexOf('=');
					if (sep > 0)
						imageExtension[line[..sep]] = line[(sep + 1)..];
				}

			Parallel.ForEach(members, member =>
			{
				var memberURLFile = config.getImageListFilePath(member);
				var newMemberURLFile = config.getCorrectedImageListFilePath(member);

				if (File.Exists(memberURLFile) && !File.Exists(newMemberURLFile))
				{
					string[] lines = File.ReadAllLines(memberURLFile);
					var newLines = new List<string>(lines.Length);
					foreach (string line in lines)
						if (line.StartsWith("http") && line.Contains("twimg.com"))
						{
							newLines.Add(line);
							if (!extensionFileExists && line.Contains("pbs.twimg.com")/* && line.Contains("?format=")*/)
								imageExtension[line[(line.LastIndexOf('/') + 1)..line.IndexOf('?')]] = line[(line.IndexOf("format=") + 7)..line.IndexOf('&')];
						}
					File.WriteAllLines(newMemberURLFile, newLines);

					Console.WriteLine($"Corrected image list for {member}.");
				}
			});

			if (!extensionFileExists)
				File.WriteAllLines(extensionFileName, imageExtension.Select(pair => $"{pair.Key}={pair.Value}"));

			Console.WriteLine("Finished correcting all image list.");

			Parallel.ForEach(members, member =>
			{
				var fileName = config.getCorrectedImageListFilePath(member);

				if (File.Exists(fileName))
				{
					var gallery_dl = new Process();
					gallery_dl.StartInfo.FileName = config.aria2Executable;
					gallery_dl.StartInfo.Arguments = config.getAria2Parameter(member); //$"-j 16 -x 4 -k 2M -s 8 --file-allocation=falloc -i \"{fileName}\" -d \".\\downloaded\\{member}\""
					gallery_dl.StartInfo.UseShellExecute = true;

					gallery_dl.Start();
					gallery_dl.WaitForExit();
				}
			});

			Console.WriteLine("Finished downloading.");

			Parallel.ForEach(members, member =>
			{
				string dir = config.getDestinationFolder(member);
				if (Directory.Exists(dir))
					foreach (string file in Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly))
					{
						var filename = file[(file.LastIndexOf('\\') + 1)..];
						if (!filename.Contains('.') && imageExtension.TryGetValue(filename, out string? extension))
						{
							var destFileName = $"{file}.{extension}";
							if (!File.Exists(destFileName))
							{
								File.Move(file, destFileName, false);
								Console.WriteLine($"Renamed {filename} to {filename}.{extension}");
							}
						}
					}
			});

			Console.WriteLine("Finished renaming.");
		}

		private static bool checkFileExistence(string fileName, string description)
		{
			if (!File.Exists(fileName))
			{
				printError($"File {fileName}({description}) not found");
				return true;
			}
			return false;
		}

		private static void printError(string message)
		{
			ConsoleColor prevColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ForegroundColor = prevColor;
		}
	}
}