using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterDump
{
	public static class Utils
	{
		public static string ToFileName(this string str) => string.Join("_", str.Split(Path.GetInvalidFileNameChars()));
	}
}
