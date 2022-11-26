namespace TwitterDumpTest
{
	[TestClass]
	public class Rule34PahealProtocolTest
	{
		[TestMethod]
		public void Initialization_ByName()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByName("Rule34Paheal"), typeof(Rule34PahealProtocol), "ByName");
		}

		[TestMethod]
		public void Initialization_ByPattern_NoProtocol_PostList()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("rule34.paheal.net/post/list/abcd"), typeof(Rule34PahealProtocol), "ByPattern - Simple URL");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("rule34.paheal.net/post/list/abcd?a=b"), typeof(Rule34PahealProtocol), "ByPattern - Simple URL with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("rule34.paheal.net/post/list/abcd?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - Simple URL with parameters");
		}

		[TestMethod]
		public void Initialization_ByPattern_NoProtocol_PostView()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("rule34.paheal.net/post/view/123456"), typeof(Rule34PahealProtocol), "ByPattern - Simple URL");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("rule34.paheal.net/post/view/123456?a=b"), typeof(Rule34PahealProtocol), "ByPattern - Simple URL with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("rule34.paheal.net/post/view/123456?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - Simple URL with parameters");
		}

		[TestMethod]
		public void Initialization_ByPattern_HTTPS_PostList()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:rule34.paheal.net/post/list/abcd"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL without slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:rule34.paheal.net/post/list/abcd?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL without slashes with with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:rule34.paheal.net/post/list/abcd?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL without slashes with parameters");

			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://rule34.paheal.net/post/list/abcd"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL with slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://rule34.paheal.net/post/list/abcd?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL with slashes with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://rule34.paheal.net/post/list/abcd?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL with slashes with parameters");
		}

		[TestMethod]
		public void Initialization_ByPattern_HTTPS_PostView()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:rule34.paheal.net/post/view/123456"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL without slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:rule34.paheal.net/post/view/123456?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL without slashes with with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https:rule34.paheal.net/post/view/123456?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL without slashes with parameters");

			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://rule34.paheal.net/post/view/123456"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL with slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://rule34.paheal.net/post/view/123456?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL with slashes with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("https://rule34.paheal.net/post/view/123456?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTPS URL with slashes with parameters");
		}

		[TestMethod]
		public void Initialization_ByPattern_HTTP_PostList()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:rule34.paheal.net/post/list/abcd"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL without slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:rule34.paheal.net/post/list/abcd?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL without slashes with with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:rule34.paheal.net/post/list/abcd?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL without slashes with parameters");

			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://rule34.paheal.net/post/list/abcd"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL with slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://rule34.paheal.net/post/list/abcd?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL with slashes with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://rule34.paheal.net/post/list/abcd?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL with slashes with parameters");
		}

		[TestMethod]
		public void Initialization_ByPattern_HTTP_PostView()
		{
			// Act and Assert
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:rule34.paheal.net/post/view/123456"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL without slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:rule34.paheal.net/post/view/123456?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL without slashes with with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http:rule34.paheal.net/post/view/123456?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL without slashes with parameters");

			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://rule34.paheal.net/post/view/123456"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL with slashes");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://rule34.paheal.net/post/view/123456?a=b"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL with slashes with a parameter");
			Assert.IsInstanceOfType(AbstractProtocol.ByPattern("http://rule34.paheal.net/post/view/123456?a=b&c=d&e=f"), typeof(Rule34PahealProtocol), "ByPattern - HTTP URL with slashes with parameters");
		}

		[TestMethod]
		public void NewFileNameRetriever_Supported()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().NewFileNameRetriever;

			// Act and Assert
			Assert.AreEqual("497994db1f951676b741341cda6ac627.jpg", Retriever("https://lotus.paheal.net/_images/497994db1f951676b741341cda6ac627/12/34/56/TheTest.jpg"), "Simple query");
			Assert.AreEqual("497994db1f951676b741341cda6ac627.png", Retriever("https://lotus.paheal.net/_images/497994db1f951676b741341cda6ac627/12/34/56/TheTest.png?a=b&c=d&efgh=ijkl&id=none&pw=%ED%ED%ED"), "Complicated query");
		}

		[TestMethod]
		public void NewFileNameRetriever_Unsupported()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().NewFileNameRetriever;

			// Act and Assert
			Assert.IsNull(Retriever("youtube.com"), "Non-CDN URL");
			Assert.IsNull(Retriever("https:youtube.com"), "Non-CDN URL");
			Assert.IsNull(Retriever("https://youtube.com"), "Non-CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/"), "Non-CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/index"), "Non-CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a"), "Non-CDN URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a&b=t"), "Non-CDN URL");
		}

		[TestMethod]
		public void MemberNameRetriever_NoProtocol_PostList()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("a", Retriever("rule34.paheal.net/post/list/a"), "Simple URL");
			Assert.AreEqual("a", Retriever("rule34.paheal.net/post/list/a?token=AAAAAA&utf8=true"), "Complicated URL");
		}

		[TestMethod]
		public void MemberNameRetriever_NoProtocol_PostView()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("1234", Retriever("rule34.paheal.net/post/view/1234"), "Simple URL");
			Assert.AreEqual("1234", Retriever("rule34.paheal.net/post/view/1234?access=123&page=3"), "Complicated URL");
		}

		[TestMethod]
		public void MemberNameRetriever_HTTPS()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("a", Retriever("https://rule34.paheal.net/post/list/a?token=AAAAAA&utf8=true"), "Complicated URL with slashes");
			Assert.AreEqual("a", Retriever("https://rule34.paheal.net/post/list/a"), "Simple URL with slashes");
			Assert.AreEqual("a", Retriever("https:rule34.paheal.net/post/list/a?token=AAAAAA&utf8=true"), "Complicated URL without slashes");
			Assert.AreEqual("a", Retriever("https:rule34.paheal.net/post/list/a"), "Simple URL without slashes");
		}

		[TestMethod]
		public void MemberNameRetriever_HTTP()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.AreEqual("a", Retriever("http://rule34.paheal.net/post/list/a?token=AAAAAA&utf8=true"), "Complicated URL with slashes");
			Assert.AreEqual("a", Retriever("http://rule34.paheal.net/post/list/a"), "Simple URL with slashes");
			Assert.AreEqual("a", Retriever("http:rule34.paheal.net/post/list/a?token=AAAAAA&utf8=true"), "Complicated URL without slashes");
			Assert.AreEqual("a", Retriever("http:rule34.paheal.net/post/list/a"), "Simple URL without slashes");
		}

		[TestMethod]
		public void MemberNameRetriever_Unsupported()
		{
			// Arrange
			Func<string, string?> Retriever = new Rule34PahealProtocol().MemberNameRetriever;

			// Act and Assert
			Assert.IsNull(Retriever("youtube.com"), "Non-compliant URL");
			Assert.IsNull(Retriever("https:youtube.com"), "Non-compliant URL");
			Assert.IsNull(Retriever("https://youtube.com"), "Non-compliant URL");
			Assert.IsNull(Retriever("https://youtube.com/"), "Non-compliant URL");
			Assert.IsNull(Retriever("https://youtube.com/index"), "Non-compliant URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a"), "Non-compliant URL");
			Assert.IsNull(Retriever("https://youtube.com/index?q=a&b=t"), "Non-compliant URL");
		}
	}
}