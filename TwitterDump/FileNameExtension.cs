using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterDump
{
	public static class FileNameExtension
	{
		public static string ToFileName(this string str) => string.Join("_", str.Split(Path.GetInvalidFileNameChars()));

		public static string ExtractFileName(this string str)
		{
			string url = str.Replace('\\', '/');
			return url.Contains('?') ? url[(url.LastIndexOf('/') + 1)..url.IndexOf('?')] : url[(url.LastIndexOf('/') + 1)..];
		}
	}
}
