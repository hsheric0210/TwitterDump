using System.Text.RegularExpressions;

namespace TwitterDump.Protocols
{
	public abstract class AbstractProtocol
	{
		private static readonly AbstractProtocol[] ProtocolRegistry = InitializeProtocolRegistry();
		public static readonly AbstractProtocol Default = new DefaultProtocol();

		private static AbstractProtocol[] InitializeProtocolRegistry()
		{
			var registry = new AbstractProtocol[2];
			registry[0] = new TwitterProtocol();
			registry[1] = new Rule34PahealProtocol();
			return registry;
		}

		public abstract string Name
		{
			get;
		}

		public abstract Regex? Pattern
		{
			get;
		}

		public abstract Func<string, string?> NewFileNameRetriever
		{
			get;
		}

		public abstract Func<string, string?> MemberNameRetriever
		{
			get;
		}

		public static AbstractProtocol? ByName(string name) => (from protocol in ProtocolRegistry where string.Equals(protocol.Name, name, StringComparison.InvariantCultureIgnoreCase) select protocol).FirstOrDefault();

		public static AbstractProtocol? ByPattern(string url) => (from protocol in ProtocolRegistry
																  where protocol.Pattern?.IsMatch(url) ?? false
																  select protocol).FirstOrDefault();

		public override int GetHashCode() => HashCode.Combine(Name.GetHashCode(), Pattern?.GetHashCode());
	}
}
