namespace TwitterDumpTest
{
	[TestClass]
	public class CmdLineExtensionTest
	{
		[TestMethod]
		public void HasCmdSwitch_SignCharPrefix()
		{
			// Arrange
			string[] args_1 = { "s", "multi", "-a", "-bcdefg", "nothing-here", "n-" };

			// Act and Assert
			Assert.IsTrue(args_1.HasCmdSwitch("a"), "Single character argument");
			Assert.IsTrue(args_1.HasCmdSwitch("bcdefg"), "Multiple characters argument");

			Assert.IsFalse(args_1.HasCmdSwitch("nothing-here"), "Multiple characters non-argument contains '-' char #1");
			Assert.IsFalse(args_1.HasCmdSwitch("here"), "Multiple characters non-argument contains '-' char #2");

			Assert.IsFalse(args_1.HasCmdSwitch("n-"), "Single character non-argument contains '-' char");
		}

		[TestMethod]
		public void HasCmdSwitch_SlashCharPrefix()
		{
			// Arrange
			string[] args_1 = { "g", "csharp", "-x", "-gi", "rick/rolled", "f/" };

			// Act and Assert
			Assert.IsTrue(args_1.HasCmdSwitch("x"), "Single character argument");
			Assert.IsTrue(args_1.HasCmdSwitch("gi"), "Multiple characters argument");

			Assert.IsFalse(args_1.HasCmdSwitch("rick/rolled"), "Multiple characters non-argument contains '/' char #1");
			Assert.IsFalse(args_1.HasCmdSwitch("rolled"), "Multiple characters non-argument contains '/' char #2");

			Assert.IsFalse(args_1.HasCmdSwitch("f/"), "Single character non-argument contains '/' char");
		}

		[TestMethod]
		public void GetCmdSwitch_SignCharPrefix()
		{
			// Arrange
			string[] args_1 = { "e", "-a", "cs", "-ternary", "-cC:\\pagefile.sys" };

			// Act and Assert
			Assert.AreEqual("", args_1.GetCmdSwitch("a", "notfound"), "Single character argument");
			Assert.AreEqual("ernary", args_1.GetCmdSwitch("t", "notfound"), "Multiple characters argument");

			Assert.AreEqual("C:\\pagefile.sys", args_1.GetCmdSwitch("c", "notfound"), "Multiple characters argument (path)");

			Assert.AreEqual("notfound", args_1.GetCmdSwitch("e", "notfound"), "Single character non-argument");
			Assert.AreEqual("notfound", args_1.GetCmdSwitch("cs", "notfound"), "Multiple characters non-argument");

			Assert.AreEqual("notfound", args_1.GetCmdSwitch("x", "notfound"), "Single character inexistent argument");
			Assert.AreEqual("notfound", args_1.GetCmdSwitch("ge", "notfound"), "Multiple characters inexistent argument");
		}

		[TestMethod]
		public void GetCmdSwitch_SlashCharPrefix()
		{
			// Arrange
			string[] args_1 = { "h", "-a", "wo", "/quater", "/eC:\\swapfile.sys" };

			// Act and Assert
			Assert.AreEqual("", args_1.GetCmdSwitch("a", "notfound"), "Single character argument");
			Assert.AreEqual("uater", args_1.GetCmdSwitch("q", "notfound"), "Multiple characters argument");

			Assert.AreEqual("C:\\swapfile.sys", args_1.GetCmdSwitch("e", "notfound"), "Multiple characters argument (path)");

			Assert.AreEqual("notfound", args_1.GetCmdSwitch("h", "notfound"), "Single character non-argument");
			Assert.AreEqual("notfound", args_1.GetCmdSwitch("wo", "notfound"), "Multiple characters non-argument");

			Assert.AreEqual("notfound", args_1.GetCmdSwitch("u", "notfound"), "Single character inexistent argument");
			Assert.AreEqual("notfound", args_1.GetCmdSwitch("az", "notfound"), "Multiple characters inexistent argument");
		}
	}
}