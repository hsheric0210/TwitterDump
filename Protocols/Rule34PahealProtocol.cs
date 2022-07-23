using System.Text.RegularExpressions;

namespace TwitterDump.Protocols
{
	public class Rule34PahealProtocol : AbstractProtocol
	{
		public override string Name => "Rule34Paheal";

		public override Regex? Pattern => new(@"(?:https?\:)?(?:\/\/)?(?:[\w]+)?\.paheal\.net\/_images\/([\w\d]+)\/.*\.([\w\d]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public override Func<string, string?> NewFileNameRetriever => (string url) =>
		{
			Match match = Pattern!.Match(url);
			if (match.Success)
				return $"{match.Groups[1].Value}.{match.Groups[2].Value}";
			return null;
		};
	}
}
