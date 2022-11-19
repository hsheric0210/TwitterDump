using System.Text.RegularExpressions;

namespace TwitterDump.Protocols
{
	public class DefaultProtocol : AbstractProtocol
	{
		public override string Name => "Default";

		public override Regex? Pattern => null;

		public override Func<string, string?> NewFileNameRetriever => (string _) => null;

		public override Func<string, string?> MemberNameRetriever => (string _) => null;
	}
}
