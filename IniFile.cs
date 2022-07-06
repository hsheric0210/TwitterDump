using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
namespace TwitterDump
{
	// revision 11 + 1
	public class IniFile
	{
		private readonly string Path;
		private readonly string EXE = Assembly.GetExecutingAssembly().GetName().Name ?? "Program";

		[DllImport("kernel32", CharSet = CharSet.Unicode)]
		private static extern long WritePrivateProfileString(string Section, string? Key, string? Value, string FilePath);

		[DllImport("kernel32", CharSet = CharSet.Unicode)]
		private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

		[DllImport("kernel32")]
		private static extern int GetLastError();

		public IniFile(string? IniPath = null) => Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;

		public string Read(string Key, string? Section = null, bool silent = false)
		{
			var RetVal = new StringBuilder(1024);
			_ = GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 1024, Path);
			if (!silent)
			{
				int lastError = GetLastError();
				if (lastError != 0)
					throw new AggregateException($"Failed to get value from configuration: Key={Key}, Section={Section ?? "null"}, Error=0x{lastError:X8}");
			}
			return RetVal.ToString().Trim();
		}

		public void Write(string? Key, object? Value, string? Section = null) => WritePrivateProfileString(Section ?? EXE, Key, Value?.ToString()?.Trim(), Path);

		public void DeleteKey(string Key, string? Section = null) => Write(Key, null, Section ?? EXE);

		public void DeleteSection(string? Section = null) => Write(null, null, Section ?? EXE);

		public bool KeyExists(string Key, string? Section = null) => Read(Key, Section, silent: true).Length > 0;
	}
}