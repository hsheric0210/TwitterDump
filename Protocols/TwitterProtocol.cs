using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterDump.Protocols
{
	public class TwitterProtocol : AbstractProtocol
	{
		public override string Name => "Twitter";
		public override Regex? Pattern => new(@"(http:|https:)?(\/\/)?twitter\.com\/.*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		public override Func<string, string?> NewFileNameRetriever => (string url) => url.Contains("pbs.twimg.com") ? $"{url[(url.LastIndexOf('/') + 1)..url.IndexOf('?')]}.{url[(url.IndexOf("format=") + 7)..url.IndexOf('&')]}" : null;
	}
}
