using System.Collections.Concurrent;
using System.Text;

namespace TwitterDump
{
	public class Config
	{
		private readonly IniFile config;


		public string galleryDLExecutable;
		public string galleryDLParameters;

		public string aria2Executable;
		public string aria2Parameters;

		public string imageListFolder;
		public string imageListFile;
		public string correctedImageListFile;

		public string extensionDictionaryFileName;
		public string listFileName;
		public string destinationFolder;

		public bool twitterMode;

		private string imageListFormat;

		public Config(IniFile config)
		{
			this.config = config;

			galleryDLExecutable = config.read("Executable", "GalleryDL");
			galleryDLParameters = config.read("Parameters", "GalleryDL");

			aria2Executable = config.read("Executable", "Aria2");
			aria2Parameters = config.read("Parameters", "Aria2");

			imageListFolder = config.read("ImageListFolder", "ImageURLList");
			imageListFile = config.read("ImageListFile", "ImageURLList");
			correctedImageListFile = config.read("CorrectedImageListFile", "ImageURLList");

			extensionDictionaryFileName = config.read("ExtensionDictionary", "Misc");
			listFileName = config.read("ListFile", "Misc");
			destinationFolder = config.read("DestinationFolder", "Misc");

			twitterMode = config.read("TwitterMode", "Misc").Equals("true");

			imageListFormat = $"{imageListFolder}\\{{0}}";
		}

		public string getImageListFilePath(string memberID) => string.Format(imageListFormat, string.Format(imageListFile, memberID));

		public string getCorrectedImageListFilePath(string memberID) => twitterMode ? string.Format(imageListFormat, string.Format(correctedImageListFile, memberID)) : getImageListFilePath(memberID);

		public string getGalleryDLParameter(string memberID) => string.Format(galleryDLParameters, memberID);

		public string getAria2Parameter(string memberID) => string.Format(aria2Parameters, getCorrectedImageListFilePath(memberID), getDestinationFolder(memberID));

		public string getDestinationFolder(string memberID) => string.Format(destinationFolder, memberID);

		public static void saveDefaults(string path)
		{
			var builder = new StringBuilder();

			builder.AppendLine("[GalleryDL]");
			
			builder.AppendLine("; gallery-dl executable name");
			builder.AppendLine("Executable=gallery-dl.exe");
			
			builder.AppendLine("; gallery-dl parameters");
			builder.AppendLine("Parameters=-G --no-download https://twitter.com/{0}");

			builder.AppendLine("[Aria2]");

			builder.AppendLine("; aria2 executable name");
			builder.AppendLine("Executable=aria2c.exe");

			builder.AppendLine("; aria2 parameters");
			builder.AppendLine("Parameters=-j 16 -x 4 -k 2M -s 8 --conditional-get true --allow-overwrite true --auto-file-renaming false --uri-selector=inorder -i {0} -d {1}");

			builder.AppendLine("[ImageURLList]");

			builder.AppendLine("; Image URL list folder");
			builder.AppendLine("ImageListFolder=ImageList");

			builder.AppendLine("; Image URL list path format");
			builder.AppendLine("ImageListFile={0}.txt");

			builder.AppendLine("; (Corrected) Image URL list path format");
			builder.AppendLine("CorrectedImageListFile={0}.lst");

			builder.AppendLine("[Misc]");

			builder.AppendLine("; The file contains image extension dictionary");
			builder.AppendLine("ExtensionDictionary=ExtensionDictionary.txt");

			builder.AppendLine("; The file contains member IDs");
			builder.AppendLine("ListFile=list.txt");

			builder.AppendLine("; Destination folder");
			builder.AppendLine("DestinationFolder=.\\Downloaded\\{0}");

			builder.AppendLine("; Twitter mode: Download media only via twimg.net; Append correct file extension to downloaded images after download phase is finished");
			builder.AppendLine("TwitterMode=true");

			File.WriteAllText(path, builder.ToString());
		}
	}
}
