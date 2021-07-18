using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestEnd2End
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Both")]
		[TestMethod]
		public void TestEndToEnd()
		{
			string input1 = "5 + ((1 + 2) * 4) - 3";
			string input2 = "9 - 12 + 15 * 3 + 2 - 5 - 19";
			string input3 = "(8 * 12 / 2 + 16 + 5) / 23";
			string input4 = "18 + 16 * 1 - 10 / 18 - 2 + 16";
			string input5 = "3 / 10 / 12 + 7 * 13 / 8 + 9";
			string input6 = "11 / 3 * 15 * 6 / 7 / 13 + 18";

			string expectingPostfix1 = "5 1 2 + 4 * + 3 -";
			string expectingPostfix2 = "";
			string expectingPostfix3 = "";
			string expectingPostfix4 = "";
			string expectingPostfix5 = "";
			string expectingPostfix6 = "";

			int expecting1 = 14;
			int expecting2 = 20;
			int expecting3 = 3;
			int expecting4 = 20;
			int expecting5 = 20;
			int expecting6 = 20;

			string postfix1 = PostfixNotation.Convert(input1);
			string postfix2 = PostfixNotation.Convert(input2);
			string postfix3 = PostfixNotation.Convert(input3);
			string postfix4 = PostfixNotation.Convert(input4);
			string postfix5 = PostfixNotation.Convert(input5);
			string postfix6 = PostfixNotation.Convert(input6);

			int result1 = PostfixNotation.Evaluate(postfix1);
			int result2 = PostfixNotation.Evaluate(postfix2);
			int result3 = PostfixNotation.Evaluate(postfix3);
			int result4 = PostfixNotation.Evaluate(postfix4);
			int result5 = PostfixNotation.Evaluate(postfix5);
			int result6 = PostfixNotation.Evaluate(postfix6);

			TestContext.WriteLine($"{input1} => {result1}");
			TestContext.WriteLine($"{input2} => {result2}");
			TestContext.WriteLine($"{input3} => {result3}");
			TestContext.WriteLine($"{input4} => {result4}");
			TestContext.WriteLine($"{input5} => {result5}");
			TestContext.WriteLine($"{input6} => {result6}");

			TestContext.WriteLine(Environment.NewLine);

			Assert.AreEqual(expectingPostfix1, postfix1, "#1");
			//Assert.AreEqual(expectingPostfix2, postfix2, "#2");
			//Assert.AreEqual(expectingPostfix3, postfix3, "#3");
			//Assert.AreEqual(expectingPostfix4, postfix4, "#4");
			//Assert.AreEqual(expectingPostfix5, postfix5, "#5");
			//Assert.AreEqual(expectingPostfix6, postfix6, "#6");

			TestContext.WriteLine(Environment.NewLine);

			Assert.AreEqual(expecting1, result1, "#1");
			Assert.AreEqual(expecting2, result2, "#2");
			Assert.AreEqual(expecting3, result3, "#3");
			Assert.AreEqual(expecting4, result4, "#4");
			Assert.AreEqual(expecting5, result5, "#5");
			Assert.AreEqual(expecting6, result6, "#6");
		}

	}
}



