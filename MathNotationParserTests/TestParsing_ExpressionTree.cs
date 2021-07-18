using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;
using System.Linq.Expressions;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestParsing_ExpressionTree
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test001()
		{
			string input = "12*x + 1";
			string expecting = "12 x * 1 +";

			string result = PostfixNotation.Convert(input);
			TestContext.WriteLine($"Input: {input}");
			TestContext.WriteLine("=>");
			TestContext.WriteLine($"Result: {result}");
			TestContext.WriteLine($"Expect: {expecting}");
			TestContext.WriteLine("");
			Assert.AreEqual(expecting, result, "#1");

			var expression = ExpressionTree.Convert(result);
			TestContext.WriteLine($"{expression}");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test002()
		{
			string input = "144*x*y + 12*y - 12*x - 3218148";
			string expecting = "144 x * y * 12 y * 12 x * 3218148 - - +";

			string result = PostfixNotation.Convert(input);
			TestContext.WriteLine($"Input: {input}");
			TestContext.WriteLine("=>");
			TestContext.WriteLine($"Result: {result}");
			TestContext.WriteLine($"Expect: {expecting}");
			TestContext.WriteLine("");
			Assert.AreEqual(expecting, result, "#2");

			var expression = ExpressionTree.Convert(result);
			TestContext.WriteLine($"{expression}");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test003()
		{
			string input = "(12*x+1)*(12*y-1)";
			string expecting = "12 x * 1 + 12 y * 1 - *";

			string result = PostfixNotation.Convert(input);
			TestContext.WriteLine($"Input: {input}");
			TestContext.WriteLine("=>");
			TestContext.WriteLine($"Result: {result}");
			TestContext.WriteLine($"Expect: {expecting}");
			TestContext.WriteLine("");
			Assert.AreEqual(expecting, result, "#3");

			var expression = ExpressionTree.Convert(result);
			TestContext.WriteLine($"{expression}");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test004()
		{
			string input = "4 / 4 * 12 - 16 / 17 + 8 - 16";
			string expecting = "4 4 / 12 * 16 17 / 8 + 16 - -";

			string result = PostfixNotation.Convert(input);
			TestContext.WriteLine($"Input: {input}");
			TestContext.WriteLine("=>");
			TestContext.WriteLine($"Result: {result}");
			TestContext.WriteLine($"Expect: {expecting}");
			TestContext.WriteLine("");
			Assert.AreEqual(expecting, result, "#4");

			var expression = ExpressionTree.Convert(result);
			TestContext.WriteLine($"{expression}");
		}
	}
}

