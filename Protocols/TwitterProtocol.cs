using System.Text.RegularExpressions;

namespace TwitterDump.Protocols
{
	public class TwitterProtocol : AbstractProtocol
	{
		public override string Name => "Twitter";

		public override Regex? Pattern => new(@"(?:https?\:)?(?:\/\/)?twitter\.com\/([\w]+)(?:[\?\/].*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public override Func<string, string?> NewFileNameRetriever => (string cdnURL) => cdnURL.Contains("pbs.twimg.com") ? $"{cdnURL[(cdnURL.LastIndexOf('/') + 1)..cdnURL.IndexOf('?')]}.{cdnURL[(cdnURL.IndexOf("format=") + 7)..cdnURL.IndexOf('&')]}" : null;

		public override Func<string, string?> MemberNameRetriever => (string memberURL) =>
		{
			Match match = Pattern!.Match(memberURL);
			if (match.Success)
				return $"{match.Groups[1].Value}";
			return null;
		};
	}
}
