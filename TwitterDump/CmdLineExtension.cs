namespace TwitterDump
{
	public static class CmdLineExtension
	{
		public static bool HasCmdSwitch(this string[] cmdLine, string switchStr)
		{
			return cmdLine.Any(arg => arg.StartsWith("-" + switchStr, StringComparison.OrdinalIgnoreCase) || arg.StartsWith("/" + switchStr, StringComparison.OrdinalIgnoreCase));
		}

		public static string GetCmdSwitch(this string[] cmdLine, string switchStr, string defaultValue)
		{
			return (from arg in cmdLine where arg.StartsWith("-" + switchStr, StringComparison.OrdinalIgnoreCase) || arg.StartsWith("/" + switchStr, StringComparison.OrdinalIgnoreCase) select arg[(switchStr.Length + 1)..]).FirstOrDefault(defaultValue);
		}
	}
}
