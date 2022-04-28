using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
namespace TwitterDump
{
	public class IniFile   // revision 11
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

		public string read(string Key, string? Section = null, string DefaultValue = "")
		{
			var RetVal = new StringBuilder(255);
			GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
			int lastError = GetLastError();
			if (lastError != 0)
				throw new Exception($"Failed to get value from configuration: Key={Key}, Section={Section ?? "null"}, Error=0x{lastError:X8}");
			return RetVal?.ToString()?.Trim() ?? DefaultValue;
		}

		public void write(string? Key, string? Value, string? Section = null) => WritePrivateProfileString(Section ?? EXE, Key, Value?.Trim(), Path);

		public void deleteKey(string Key, string? Section = null) => write(Key, null, Section ?? EXE);

		public void deleteSection(string? Section = null) => write(null, null, Section ?? EXE);

		public bool keyExists(string Key, string? Section = null)
		{
			var RetVal = new StringBuilder(255);
			GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
			int lastError = GetLastError();
			if (lastError != 0)
				return false;
			return RetVal.ToString().Trim().Length > 0;
		}
	}
}