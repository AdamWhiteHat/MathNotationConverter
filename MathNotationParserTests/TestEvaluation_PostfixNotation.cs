using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestEvaluation_PostfixNotation
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Evaluation")]
		[TestMethod]
		public void Test001()
		{
			string input1 = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
			int expecting1 = 3;
			int result1 = PostfixNotation.Evaluate(input1);
			TestContext.WriteLine($"{input1} => {result1}");
			Assert.AreEqual(expecting1, result1, "#1");
		}

		[TestCategory("Evaluation")]
		[TestMethod]
		public void Test002()
		{
			string input2 = "16 3 1 / 1 + 12 14 / 9 / - +";
			int expecting2 = 20;
			int result2 = PostfixNotation.Evaluate(input2);
			TestContext.WriteLine($"{input2} => {result2}");
			Assert.AreEqual(expecting2, result2, "#2");
		}

		[TestCategory("Evaluation")]
		[TestMethod]
		public void Test003()
		{
			string input3 = "4 4 / 12 * 16 17 / 8 + 16 - -";
			int expecting3 = 20;
			int result3 = PostfixNotation.Evaluate(input3);
			TestContext.WriteLine($"{input3} => {result3}");
			Assert.AreEqual(expecting3, result3, "#3");
		}
	}
}

