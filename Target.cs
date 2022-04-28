using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterDump.Protocols;

namespace TwitterDump
{
	public class Target
	{
		public AbstractProtocol protocol;
		public string ID;

		public Target(string raw)
		{
			if (raw.Contains('|'))
			{
				string[] pieces = raw.Split('|');
				ID = pieces[1];
				protocol = AbstractProtocol.ByName(pieces[0]) ?? AbstractProtocol.ByPattern(ID) ?? AbstractProtocol.Default;
			}
			else
			{
				ID = raw;
				protocol = AbstractProtocol.ByPattern(raw) ?? AbstractProtocol.Default;
			}
		}

		public override int GetHashCode() => HashCode.Combine(protocol.GetHashCode(), ID.GetHashCode());
	}
}
