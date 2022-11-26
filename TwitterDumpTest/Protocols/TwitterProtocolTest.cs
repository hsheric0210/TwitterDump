namespace TwitterDumpTest
{
	[TestClass]
	public class TwitterProtocolTest
	{
		[TestMethod]
		public void Initialization_ByName()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByName("Twitter"), typeof(TwitterProtocol), "ByName");
		}

		[TestMethod]
		public void Initialization_ByPattern_NoProtocol()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("twitter.com/things"), typeof(TwitterProtocol), "ByPattern - Simple URL");
		}

		[TestMethod]
		public void Initialization_ByPattern_HTTPS()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://twitter.com/things?token=TheTokens&secondaryToken=ABCD"), typeof(TwitterProtocol), "ByPattern - Full HTTPS URL with parameters");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://twitter.com/things?token=TheTokens"), typeof(TwitterProtocol), "ByPattern - Full HTTPS URL with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://twitter.com/things"), typeof(TwitterProtocol), "ByPattern - Simple full HTTPS URL");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:twitter.com/things"), typeof(TwitterProtocol), "ByPattern - Simple HTTPS URL");
		}

		[TestMethod]
		public void Initialization_ByPattern_HTTP()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://twitter.com/things?token=TheTokens&secondaryToken=ABCD"), typeof(TwitterProtocol), "ByPattern - Full HTTP URL with parameters");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://twitter.com/things?token=TheTokens"), typeof(TwitterProtocol), "ByPattern - Full HTTP URL with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://twitter.com/things"), typeof(TwitterProtocol), "ByPattern - Simple full HTTP URL");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:twitter.com/things"), typeof(TwitterProtocol), "ByPattern - Simple HTTP URL");
		}

		[TestMethod]
		public void NewFileNameRetriever_Supported()
		{
			// Arrange
			Func<string, string?> Retriever = new TwitterProtocol().NewFileNameRetriever;

			// Act and Assert
			Assert.AreEqual("abcd.jpg", Retriever("https://pbs.twimg.com/abcd?format=jpg&time=100"), "Simple query");
			Assert.AreEqual("efgh.png", Retriever("https://pbs.twimg.com/efgh?a=b&c=d&format=png&e=f&g=h&time=100"), "Complicated query");
		}

		[TestMethod]
		public void NewFileNameRetriever_Unsupported()
		{
			// Arrange
			Func<string, string?> Retriever = new TwitterProtocol().NewFileNameRetriever;

			// Act and Assert
			Assert.IsNull(Retriever("youtube.com"), "Non-twitter CDN URL");
			Assert.IsNull(Retriever("https:youtube.com"), "Non-twitter CDN URL");
			Assert.IsNull(Retriever("https://youtube.com"), "Non-twitter CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/"), "Non-twitter CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/index"), "Non-twitter CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a"), "Non-twitter CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a&b=t"), "Non-twitter CDN URL");
		}

		[TestMethod]
		public void MemberNameRetriever_NoProtocol()
		{
			// Arrange
			Func<string, string?> Retriever = new TwitterProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("ghij", Retriever("twitter.com/ghij"), "Simple URL");
		}

		[TestMethod]
		public void MemberNameRetriever_HTTPS()
		{
			// Arrange
			Func<string, string?> Retriever = new TwitterProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("efgh", Retriever("https://twitter.com/efgh?token=TheTokens&secondaryToken=ABCD"), "Full HTTPS URL with parameters");
			Assert.AreEqual("mnop", Retriever("https://twitter.com/mnop?token=TheTokens"), "Full HTTPS URL with a parameter");
			Assert.AreEqual("uvwx", Retriever("https://twitter.com/uvwx"), "Simple full HTTPS URL");
			Assert.AreEqual("cdef", Retriever("https:twitter.com/cdef"), "Simple HTTPS URL");
		}

		[TestMethod]
		public void MemberNameRetriever_HTTP()
		{
			// Arrange
			Func<string, string?> Retriever = new TwitterProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("abcd", Retriever("http://twitter.com/abcd?token=TheTokens&secondaryToken=ABCD"), "Full HTTP URL with parameters");
			Assert.AreEqual("ijkl", Retriever("http://twitter.com/ijkl?token=TheTokens"), "Full HTTP URL with a parameter");
			Assert.AreEqual("qrst", Retriever("http://twitter.com/qrst"), "Simple full HTTP URL");
			Assert.AreEqual("yzab", Retriever("http:twitter.com/yzab"), "Simple HTTP URL");
		}


		[TestMethod]
		public void MemberNameRetriever_Unsupported()
		{
			// Arrange
			Func<string, string?> Retriever = new TwitterProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.IsNull(Retriever("youtube.com"), "Non-twitter URL");
			Assert.IsNull(Retriever("https:youtube.com"), "Non-twitter URL");
			Assert.IsNull(Retriever("https://youtube.com"), "Non-twitter URL");
			Assert.IsNull(Retriever("https://youtube.com/"), "Non-twitter URL");
			Assert.IsNull(Retriever("https://youtube.com/index"), "Non-twitter URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a"), "Non-twitter URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a&b=t"), "Non-twitter URL");
		}
	}
}