using System.Text.RegularExpressions;

namespace TwitterDump.Protocols
{
	public class Rule34PahealProtocol : AbstractProtocol
	{
		public override string Name => "Rule34Paheal";

		public override Regex? Pattern => new(@"(?:https?\:)?(?:\/\/)?rule34\.paheal\.net\/post\/(?:list|view)\/([\w]+)(?:\/[\d]+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private Regex? CdnPattern => new(@"(?:https?\:)?(?:\/\/)?(?:[\w]+)?\.paheal\.net\/_images\/([\w]+)\/.*\.([\w]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public override Func<string, string?> NewFileNameRetriever => (string cdnURL) =>
		{
			Match match = CdnPattern!.Match(cdnURL);
			if (match.Success)
				return $"{match.Groups[0].Value}.{match.Groups[1].Value}";
			return null;
		};

		public override Func<string, string?> MemberNameRetriever => (string _) => null;
	}
}
