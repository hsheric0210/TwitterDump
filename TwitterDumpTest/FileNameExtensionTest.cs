namespace TwitterDumpTest
{
	[TestClass]
	public class FileNameExtensionTest
	{
		[TestMethod]
		public void ToFileName_NoChange()
		{
			// Act and Assert
			Assert.AreEqual("abcd", "abcd".ToFileName(), "Simple string");
			Assert.AreEqual("r.i.c.k", "r.i.c.k".ToFileName(), "'.' chars");
		}

		[TestMethod]
		public void ToFileName_Replace()
		{
			// Act and Assert
			Assert.AreEqual("a_b_c_d", "a/b/c/d".ToFileName(), "'/' chars");
			Assert.AreEqual("r_i_c_k", "r\\i\\c\\k".ToFileName(), "'\\' chars");
			Assert.AreEqual("t_w_i_t", "t*w<i>t".ToFileName(), "Multiple disallowed chars");
		}

		[TestMethod]
		public void ExtractFileName_Path()
		{
			// Act and Assert
			Assert.AreEqual("dfb421800fadb764b1a67d3a8bc0e25c.mp4", "dfb421800fadb764b1a67d3a8bc0e25c.mp4".ExtractFileName(), "Simple filename");
			Assert.AreEqual("ace1134719b5e954e3011f2128e52b7f.bin", "..\\..\\..\\abcd\\efgh\\ace1134719b5e954e3011f2128e52b7f.bin".ExtractFileName(), "Relative path");
			Assert.AreEqual("e28c159c7048286016a98d7a5ef115de.bin", "C:\\Things\\ABCD\\e28c159c7048286016a98d7a5ef115de.bin".ExtractFileName(), "Absolute path");
		}

		[TestMethod]
		public void ExtractFileName_URL()
		{

			// Act and Assert
			Assert.AreEqual("ED82DE63F680CEADAA4D257B463F7655.mp4", "http://exk7sz9a8yvdllql.com/ED82DE63F680CEADAA4D257B463F7655.mp4".ExtractFileName(), "Simple HTTP url");
			Assert.AreEqual("71E207D47B65B617752A4EFFAFE7A523.txt", "http://1agnxq74pvot5d1d.com/71E207D47B65B617752A4EFFAFE7A523.txt?access=rknx46qk8bni8fik".ExtractFileName(), "HTTP url with a parameter");
			Assert.AreEqual("03509909BD4E6E6C42353ED90C4759F5.php", "http://0a9y9omyj3jup4sx.org/03509909BD4E6E6C42353ED90C4759F5.php?accesstoken=ukrt2zyss3sxa7ji&from=v1c6mp79np1eh9kv&token=kcn2ad209o2ivn0n".ExtractFileName(), "HTTP url with parameters");

			Assert.AreEqual("C092F52CEA8E95DC9C52CB18B02ED8FC.bin", "http://qbguunedrs7wif13.org/web/www/http/things/C092F52CEA8E95DC9C52CB18B02ED8FC.bin".ExtractFileName(), "HTTP url with directories");
			Assert.AreEqual("62137F7BA0791CEAAAA42704BCAEF940.sys", "http://5n70ud55jtlmql3r.com/a/b/c/d/e/f/g/h/62137F7BA0791CEAAAA42704BCAEF940.sys?param=6lwps0yd5ymr279r".ExtractFileName(), "HTTP url with directories and a parameter");
			Assert.AreEqual("5C8D4BB40B1B14527E0E19548660364F.php", "http://we7yegmt6u9ey2fy.com/kj8dksof17q3qyzk/23xyysh7hek4u9sa/vz477065blpalxzn/5C8D4BB40B1B14527E0E19548660364F.php?param=abcdef&param2=ld082hcxegfz49tc&token=9gs5jtw2urnb2lql".ExtractFileName(), "HTTP url with directories and parameters");
		}
	}
}
