using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterDump.Protocols
{
	public abstract class AbstractProtocol
	{
		private static readonly AbstractProtocol[] ProtocolRegistry;
		public static readonly AbstractProtocol Default = new DefaultProtocol();

		static AbstractProtocol()
		{
			ProtocolRegistry = new AbstractProtocol[1];
			ProtocolRegistry[0] = new TwitterProtocol();
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

		public static AbstractProtocol? ByName(string name) => (from protocol in ProtocolRegistry where string.Equals(protocol.Name, name, StringComparison.InvariantCultureIgnoreCase) select protocol).FirstOrDefault();

		public static AbstractProtocol? ByPattern(string url) => (from protocol in ProtocolRegistry
														  where protocol.Pattern?.IsMatch(url) ?? false
														  select protocol).FirstOrDefault();

		public override int GetHashCode() => HashCode.Combine(Name.GetHashCode(), Pattern?.GetHashCode());
	}
}
