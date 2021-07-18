using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestParsing_PostfixNotation
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
			string input1 = "12*x + 1";
			string expecting1 = "12 x * 1 +";

			string result1 = PostfixNotation.Convert(input1);

			TestContext.WriteLine($"{input1} => {result1}");

			Assert.AreEqual(expecting1, result1, "#1");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test002()
		{
			string input2 = "144*x*y + 12*y - 12*x - 3218148";
			string expecting2 = "144 x * y * 12 y * 12 x * 3218148 - - +";

			string result2 = PostfixNotation.Convert(input2);

			TestContext.WriteLine($"{input2} => {result2}");

			Assert.AreEqual(expecting2, result2, "#2");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test003()
		{
			string input3 = "(12*x+1)*(12*y-1)";
			string expecting3 = "12 x * 1 + 12 y * 1 - *";

			string result3 = PostfixNotation.Convert(input3);

			TestContext.WriteLine($"{input3} => {result3}");

			Assert.AreEqual(expecting3, result3, "#3");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test004()
		{
			string input4 = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
			string expecting4 = "3 4 2 * 1 5 - 2 3 ^ ^ / +";

			string result4 = PostfixNotation.Convert(input4);

			TestContext.WriteLine($"{input4} => {result4}");

			Assert.AreEqual(expecting4, result4, "#4");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test005()
		{
			string input5 = "16 + 3 / 1 + 1 - 12 / 14 / 9 ";
			string expecting5 = "16 3 1 / 1 + 12 14 / 9 / - +";

			string result5 = PostfixNotation.Convert(input5);

			TestContext.WriteLine($"{input5} => {result5}");

			Assert.AreEqual(expecting5, result5, "#5");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test006()
		{
			string input6 = "4 / 4 * 12 - 16 / 17 + 8 - 16";
			string expecting6 = "4 4 / 12 * 16 17 / 8 + 16 - -";

			string result6 = PostfixNotation.Convert(input6);

			TestContext.WriteLine($"{input6} => {result6}");

			Assert.AreEqual(expecting6, result6, "#6");
		}
	}
}
