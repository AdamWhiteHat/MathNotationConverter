using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestManyEquationsSameValue
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Both")]
		[TestMethod]
		public void TestMany()
		{
			int expectingResult = 20;

			string[] equations = new string[]
			{
					"1 + 9 * 2 - 11 / 14 - 12 / 12" ,
					"6 + 4 + 20 + 9 - 1 - 17 - 1"   ,
					"4 + 14 - 5 + 9 + 6 - 19 + 11"  ,
					"18 - 15 / 16 / 6 * 4 / 11 - 2" ,
					"13 - 8 / 5 - 19 - 6 / 11 - 11" ,
					"2 / 15 + 4 * 8 / 10 * 6 + 2"   ,
					"18 + 16 * 1 - 10 / 18 - 2 + 16",
					"3 / 10 / 12 + 7 * 13 / 8 + 9"  ,
					"11 / 3 * 15 * 6 / 7 / 13 + 18" ,
					"6 / 4 + 6 + 11 / 13 - 6 + 19"  ,
					"3 / 4 + 7 * 16 / 5 * 11 / 12"
			};

			TestContext.WriteLine(string.Join(Environment.NewLine, equations));


			foreach (string input in equations)
			{
				string postfix = PostfixNotation.Convert(input);

				TestContext.WriteLine(postfix);

				int result = PostfixNotation.Evaluate(postfix);

				Assert.AreEqual(expectingResult, result);
			}
			TestContext.WriteLine(Environment.NewLine);
			TestContext.WriteLine(Environment.NewLine);
		}
	}
}
