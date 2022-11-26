namespace TwitterDump
{
	public static class TokenFormatterExtension
	{
		public static string FormatRetriverParameters(this string format, string memberID) => format.FormatTokens(new Dictionary<string, string>
		{
			["memberID"] = memberID,
			["memberIDFileName"] = memberID.ToFileName()
		});

		public static string FormatDownloaderParameters(this string format, string memberID, string inputFileName, string destinationFormat) => format.FormatTokens(new Dictionary<string, string>
		{
			["memberID"] = memberID,
			["memberIDFileName"] = memberID.ToFileName(),
			["inputFileName"] = inputFileName,
			["destination"] = destinationFormat.FormatDestination(memberID)
		});

		public static string FormatDestination(this string format, string memberID) => format.FormatTokens(new Dictionary<string, string>
		{
			["memberID"] = memberID,
			["memberIDFileName"] = memberID.ToFileName()
		});

		public static string FormatTokens(this string format, IDictionary<string, string> tokens)
		{
			foreach (KeyValuePair<string, string> token in tokens)
				format = format.Replace($"${{{token.Key}}}", token.Value, StringComparison.OrdinalIgnoreCase);
			return format;
		}
	}
}
