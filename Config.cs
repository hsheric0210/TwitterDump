using System.Collections.Concurrent;
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

		public string GalleryDLExecutable;
		public string GalleryDLParameters;

		public string Aria2Executable;
		public string Aria2Parameters;

		public string InputFileName;
		public string DestinationFolder;

		public Config(IniFile config)
		{
			GalleryDLExecutable = config.read("Executable", "Gallery-DL", Default_GalleryDL_Executable);
			GalleryDLParameters = config.read("Parameters", "Gallery-DL", Default_GalleryDL_Parameters);

			Aria2Executable = config.read("Executable", "Aria2", Default_Aria2_Executable);
			Aria2Parameters = config.read("Parameters", "Aria2", Default_Aria2_Parameters);

			InputFileName = config.read("ListFile", "Misc", Default_Input_File);
			DestinationFolder = config.read("DestinationFolder", "Misc", Default_Destination_Folder);
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

			File.WriteAllText(path, builder.ToString());
		}
	}
}
