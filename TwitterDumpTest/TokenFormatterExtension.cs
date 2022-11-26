namespace TwitterDumpTest
{
	[TestClass]
	public class TokenFormatterExtension
	{
		[TestMethod]
		public void TokenFormatterExtension_FormatTokens()
		{
			// Act and Assert
			Assert.AreEqual("abcd", "a${beta}c${delta}".FormatTokens(new Dictionary<string, string>()
			{
				["beta"] = "b",
				["delta"] = "d"
			}), "Single characters");
			Assert.AreEqual("you are rick rolled", "you are ${noun} ${verb}ed".FormatTokens(new Dictionary<string, string>()
			{
				["noun"] = "rick",
				["verb"] = "roll"
			}), "Multiple characters");
		}
	}
}
