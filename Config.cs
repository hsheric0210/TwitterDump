using System.Text;

namespace TwitterDump
{
	public class Config
	{
		private const string GalleryDLSection = "Gallery-DL";
		private const string GalleryDLExecutableKey = "GalleryDLExecutable";
		private const string DefaultGalleryDLExecutable = "gallery-dl.exe";
		private const string GalleryDLParametersKey = "GalleryDLParameters";
		private const string DefaultGalleryDLParameters = "-G --no-download  --write-unsupported=\"unsupported.${memberIDFileName}.txt\" \"https://twitter.com/${memberID}\"";

		private const string Aria2Section = "Aria2";
		private const string Aria2ExecutableKey = "Aria2Executable";
		private const string DefaultAria2Executable = "aria2c.exe";
		private const string Aria2ParametersKey = "Aria2Parmeters";
		private const string DefaultAria2Parameters = "-j16 -x4 -l\"aria2.${memberIDFileName}.log\" -m0 --retry-wait=10 --allow-overwrite=true --conditional-get=true -Rtrue --auto-file-renaming=false --uri-selector=inorder -i${inputFileName} -d${destination}";

		private const string MiscSection = "Misc";
		private const string InputFileKey = "ListFile";
		private const string DefaultInputFile = "list.txt";
		private const string DestinationFolderKey = "Destination";
		private const string DefaultDestinationFolder = ".\\Downloaded\\${memberIDFileName}";
		private const string ExtractorAsDownloaderKey = "ExtractorAsDownloader";
		private const bool DefaultExtractorAsDownloader = false;
		private const string ExtractorParallellismKey = "ExtractorParallellsim";
		private const int DefaultExtractorParallellism = 6;
		private const string DownloaderParallellismKey = "DownloaderParallellsim";
		private const int DefaultDownloaderParallellism = 4;

		private readonly IniFile Ini;

		public string GalleryDLExecutable => ParseString(GalleryDLExecutableKey, GalleryDLSection, DefaultGalleryDLExecutable);

		public string GalleryDLParameters => ParseString(GalleryDLParametersKey, GalleryDLSection, DefaultGalleryDLParameters);

		public string Aria2Executable => ParseString(Aria2ExecutableKey, Aria2Section, DefaultAria2Executable);

		public string Aria2Parameters => ParseString(Aria2ParametersKey, Aria2Section, DefaultAria2Parameters);

		public string InputFileName => ParseString(InputFileKey, MiscSection, DefaultInputFile);

		public string DestinationFolder => ParseString(DestinationFolderKey, MiscSection, DefaultDestinationFolder);

		public bool ExtractorAsDownloader => ParseBool(ExtractorAsDownloaderKey, MiscSection, DefaultExtractorAsDownloader);

		public int ExtractorParallellism => ParseInt(ExtractorParallellismKey, MiscSection, DefaultExtractorParallellism);

		public int DownloaderParallellism => ParseInt(DownloaderParallellismKey, MiscSection, DefaultDownloaderParallellism);

		public Config(IniFile config) => Ini = config;

		private int ParseInt(string key, string section, int defaultValue)
		{
			if (Ini.KeyExists(key, section) && int.TryParse(Ini.Read(key, section), out int result))
				return result;
			Ini.Write(key, defaultValue, section);
			return defaultValue;
		}

		private bool ParseBool(string key, string section, bool defaultValue)
		{
			if (Ini.KeyExists(key, section) && bool.TryParse(Ini.Read(key, section), out bool result))
				return result;
			Ini.Write(key, defaultValue, section);
			return defaultValue;
		}

		private string ParseString(string key, string section, string defaultValue)
		{
			if (Ini.KeyExists(key, section))
				return Ini.Read(key, section);
			Ini.Write(key, defaultValue, section);
			return defaultValue;
		}

		public string GetGalleryDLParameter(string memberID) => FormatTokens(GalleryDLParameters, new Dictionary<string, string>
		{
			["memberID"] = memberID,
			["memberIDFileName"] = memberID.ToFileName()
		});

		public string GetAria2Parameter(string memberID, string inputFileName) => FormatTokens(Aria2Parameters, new Dictionary<string, string>
		{
			["memberID"] = memberID,
			["memberIDFileName"] = memberID.ToFileName(),
			["inputFileName"] = inputFileName,
			["destination"] = GetDestinationFolder(memberID)
		});

		public string GetDestinationFolder(string memberID) => FormatTokens(DestinationFolder, new Dictionary<string, string>
		{
			["memberID"] = memberID,
			["memberIDFileName"] = memberID.ToFileName()
		});

		private static string FormatTokens(string format, IDictionary<string, string> tokens)
		{
			foreach (KeyValuePair<string, string> token in tokens)
				format = format.Replace($"${{{token.Key}}}", token.Value);
			return format;
		}

		public static void SavePrettyDefaults(string path)
		{
			var builder = new StringBuilder();

			builder.Append('[').Append(GalleryDLSection).AppendLine("]");
			builder.AppendLine("; Gallery-DL executable path.");
			builder.Append(GalleryDLExecutableKey).Append('=').AppendLine(DefaultGalleryDLExecutable);
			builder.AppendLine("; Gallery-DL parameters.");
			builder.Append(GalleryDLParametersKey).Append('=').AppendLine(DefaultGalleryDLParameters);

			builder.Append('[').Append(Aria2Section).AppendLine("]");
			builder.AppendLine("; Aria2 executable path.");
			builder.Append(Aria2ExecutableKey).Append('=').AppendLine(DefaultAria2Executable);
			builder.AppendLine("; Aria2 parameters.");
			builder.Append(Aria2ParametersKey).Append('=').AppendLine(DefaultAria2Parameters);

			builder.Append('[').Append(MiscSection).AppendLine("]");
			builder.AppendLine("; The file contains member IDs. Each IDs should be separated with line breaks.");
			builder.Append(InputFileKey).Append('=').AppendLine(DefaultInputFile);
			builder.AppendLine("; Destination folder.");
			builder.Append(DestinationFolderKey).Append('=').AppendLine(DefaultDestinationFolder);
			builder.AppendLine("; Use extractor as downloader.");
			builder.Append(ExtractorAsDownloaderKey).Append('=').AppendLine(DefaultExtractorAsDownloader.ToString());
			builder.AppendLine("; Limits the maximum count of extractor tasks which would run in parallel.");
			builder.Append(ExtractorParallellismKey).Append('=').AppendLine(DefaultExtractorParallellism.ToString());
			builder.AppendLine("; Limits the maximum count of downloader tasks which would run in parallel.");
			builder.Append(DownloaderParallellismKey).Append('=').AppendLine(DefaultDownloaderParallellism.ToString());

			File.WriteAllText(path, builder.ToString());
		}
	}
}
