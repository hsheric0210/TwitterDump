namespace TwitterDump
{
	public class Config
	{
		public string ListFile
		{
			get; set;
		} = "list.txt";

		public string Destination
		{
			get; set;
		} = ".\\Downloaded\\${memberIDFileName}";

		public bool UseRetrieverIntegratedDownloader
		{
			get; set;
		} = false;

		public bool SkipAlreadyExists
		{
			get; set;
		} = false;

		public bool KeepDownloadListFile
		{
			get; set;
		} = false;

		public RetrieverSection Retriever
		{
			get; set;
		} = new RetrieverSection();

		public DownloaderSection Downloader
		{
			get; set;
		} = new DownloaderSection();

		public ParallelismSection Parallelism
		{
			get; set;
		} = new ParallelismSection();
	}

	public class RetrieverSection
	{
		public string Executable
		{
			get; set;
		} = "gallery-dl.exe";

		public string Parameters
		{
			get; set;
		} = "-G --no-download \"https://twitter.com/${memberID}\"";
	}

	public class DownloaderSection
	{
		public string Executable
		{
			get; set;
		} = "aria2c.exe";

		public string Parameters
		{
			get; set;
		} = "-j16 -x4 -l\"aria2.${memberIDFileName}.log\" -m0 --retry-wait=10 --allow-overwrite=true --conditional-get=true -Rtrue --auto-file-renaming=false --uri-selector=inorder -i\"${inputFileName}\" -d\"${destination}\"";
	}

	public class ParallelismSection
	{
		public int RetrieverParallelism
		{
			get; set;
		} = 6;

		public int DownloaderParallelism
		{
			get; set;
		} = 4;
	}
}
