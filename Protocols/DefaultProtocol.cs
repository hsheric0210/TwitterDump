using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterDump.Protocols
{
	public class DefaultProtocol : AbstractProtocol
	{
		public override string Name => "Default";
		public override Regex? Pattern => null;
		public override Func<string, string?> NewFileNameRetriever => (string _) => null;
	}
}
