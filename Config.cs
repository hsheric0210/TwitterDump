using System.Text;

namespace TwitterDump
{
	public class Config
	{
		private const string Default_GalleryDL_Executable = "gallery-dl.exe";
		private const string Default_GalleryDL_Parameters = "-G --no-download {0}";
		private const string Default_Aria2_Executable = "aria2c.exe";
		private const string Default_Aria2_Parameters = "-j16 -x4 -k16M -s8 --log=aria2-{1}.log --allow-overwrite=true --conditional-get=true --remote-time=true --auto-file-renaming=false --uri-selector=inorder --input-file={2} -d {0}";
		private const string Default_Input_File = "list.txt";
		private const string Default_Destination_Folder = ".\\Downloaded\\{0}";
		private const string Default_ExtractorAsDownloader = "false";
		private const string Default_Parallellism = "6";

		public string GalleryDLExecutable
		{
			get; set;
		}

		public string GalleryDLParameters
		{
			get; set;
		}

		public string Aria2Executable
		{
			get; set;
		}

		public string Aria2Parameters
		{
			get; set;
		}

		public string InputFileName
		{
			get; set;
		}

		public string DestinationFolder
		{
			get; set;
		}

		public bool ExtractorAsDownloader
		{
			get; set;
		}

		public int Parallellism
		{
			get; set;
		}

		public Config(IniFile config)
		{
			GalleryDLExecutable = config.read("Executable", "Gallery-DL", Default_GalleryDL_Executable);
			GalleryDLParameters = config.read("Parameters", "Gallery-DL", Default_GalleryDL_Parameters);

			Aria2Executable = config.read("Executable", "Aria2", Default_Aria2_Executable);
			Aria2Parameters = config.read("Parameters", "Aria2", Default_Aria2_Parameters);

			InputFileName = config.read("ListFile", "Misc", Default_Input_File);
			DestinationFolder = config.read("DestinationFolder", "Misc", Default_Destination_Folder);

			ExtractorAsDownloader = Convert.ToBoolean(config.read("ExtractorAsDownloader", "Misc", Default_ExtractorAsDownloader));
			Parallellism = Convert.ToInt32(config.read("Parallellism", "Misc", Default_Parallellism));
		}

		public string GetGalleryDLParameter(string memberID) => string.Format(GalleryDLParameters, memberID);

		public string GetAria2Parameter(string memberID, string inputFileName) => string.Format(Aria2Parameters, GetDestinationFolder(memberID), memberID.ToFileName(), inputFileName);

		public string GetDestinationFolder(string memberID) => string.Format(DestinationFolder, memberID.ToFileName());

		public static void SaveDefaults(string path)
		{
			var builder = new StringBuilder();

			builder.AppendLine("[Gallery-DL]");
			builder.AppendLine("; gallery-dl executable name");
			builder.Append("Executable=").AppendLine(Default_GalleryDL_Executable);
			builder.AppendLine("; gallery-dl parameters");
			builder.Append("Parameters=").AppendLine(Default_GalleryDL_Parameters);

			builder.AppendLine("[Aria2]");
			builder.AppendLine("; aria2 executable name");
			builder.Append("Executable=").AppendLine(Default_Aria2_Executable);
			builder.AppendLine("; aria2 parameters");
			builder.Append("Parameters=").AppendLine(Default_Aria2_Parameters);

			builder.AppendLine("[Misc]");
			builder.AppendLine("; The file contains member IDs. Seperated by ';' character");
			builder.Append("ListFile=").AppendLine(Default_Input_File);
			builder.AppendLine("; Destination folder");
			builder.Append("DestinationFolder=").AppendLine(Default_Destination_Folder);
			builder.AppendLine("; Use extractor as downloader");
			builder.Append("ExtractorAsDownloader=").AppendLine(Default_ExtractorAsDownloader);
			builder.AppendLine("; The count of task which will be executed in parallel");
			builder.Append("Parallellism=").AppendLine(Default_Parallellism);

			File.WriteAllText(path, builder.ToString());
		}
	}
}
