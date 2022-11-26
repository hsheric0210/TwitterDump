using Serilog;
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
				Log.Information("Force using protocol {protocol} on entry {entry}.", protocol.Name, ID);
			}
			else
			{
				ID = raw;
				protocol = AbstractProtocol.ByPattern(raw) ?? AbstractProtocol.Default;
				Log.Information("Automatically detected protocol {protocol} on entry {entry}.", protocol.Name, ID);

				string? parsedID = protocol.MemberNameRetriever(raw);
				if (parsedID != null)
				{
					Log.Information("Parsed target id {id} from entry {entry} using protocol {protocol}.", parsedID, raw, protocol.Name);
					ID = parsedID;
				}
			}
		}

		public override int GetHashCode() => HashCode.Combine(protocol.GetHashCode(), ID.GetHashCode());
	}
}
