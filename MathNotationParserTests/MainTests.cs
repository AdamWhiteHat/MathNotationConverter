using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class MainTests
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[ClassInitializeAttribute]
		public static void Initialize(TestContext context)
		{
		}


		[TestMethod]
		public void TestShuntingYardAlgorithm()
		{
			string input1 = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
			string input2 = "16 + 3 / 1 + 1 - 12 / 14 / 9 ";
			string input3 = "4 / 4 * 12 - 16 / 17 + 8 - 16";

			string expecting1 = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
			string expecting2 = "16 3 1 / 1 + 12 14 / 9 / - +";
			string expecting3 = "4 4 / 12 * 16 17 / 8 + 16 - -";

			string result1 = ShuntingYardAlgorithm.Convert(input1);
			string result2 = ShuntingYardAlgorithm.Convert(input2);
			string result3 = ShuntingYardAlgorithm.Convert(input3);

			Assert.AreEqual(expecting1, result1);
			Assert.AreEqual(expecting2, result2);
			Assert.AreEqual(expecting3, result3);
			TestContext.WriteLine(Environment.NewLine);
		}

		[TestMethod]
		public void TestPostfixEvaluation()
		{
			string input1 = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
			string input2 = "16 3 1 / 1 + 12 14 / 9 / - +";
			string input3 = "4 4 / 12 * 16 17 / 8 + 16 - -";

			int expecting1 = 3;
			int expecting2 = 20;
			int expecting3 = 20;

			int result1 = PostfixNotation.Evaluate(input1);
			int result2 = PostfixNotation.Evaluate(input2);
			int result3 = PostfixNotation.Evaluate(input3);

			Assert.AreEqual(expecting1, result1);
			Assert.AreEqual(expecting2, result2);
			Assert.AreEqual(expecting3, result3);
		}

		[TestMethod]
		public void TestEnd2End()
		{
			string input1 = "5 + ((1 + 2) * 4) - 3";
			string input2 = "9 - 12 + 15 * 3 + 2 - 5 - 19";
			string input3 = "8 * 12 / 11 / 2 + 16 + 5 / 13";

			string input4 = "18 + 16 * 1 - 10 / 18 - 2 + 16";
			string input5 = "3 / 10 / 12 + 7 * 13 / 8 + 9";
			string input6 = "11 / 3 * 15 * 6 / 7 / 13 + 18";

			string postfix = "";
			string expectingPostfix = "5 1 2 + 4 * + 3 -";
			int result1 = 0;

			int expecting1 = 14;
			int expecting2 = 20;
			int expecting3 = 3;
			int expecting4 = 20;
			int expecting5 = 20;
			int expecting6 = 20;

			postfix = ShuntingYardAlgorithm.Convert(input1);
			result1 = PostfixNotation.Evaluate(postfix);

			Assert.AreEqual(expectingPostfix, postfix);
			Assert.AreEqual(expecting1, result1);


			int result2 = PostfixNotation.Evaluate(ShuntingYardAlgorithm.Convert(input2));
			int result3 = PostfixNotation.Evaluate(ShuntingYardAlgorithm.Convert(input3));

			TestContext.WriteLine(Environment.NewLine);
			TestContext.WriteLine(Environment.NewLine);
		}

		[TestMethod]
		public void TestManyEquationsSameValue()
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

				string postfix = ShuntingYardAlgorithm.Convert(input);

				TestContext.WriteLine(postfix);

				int result = PostfixNotation.Evaluate(postfix);

				Assert.AreEqual(expectingResult, result);
			}
			TestContext.WriteLine(Environment.NewLine);
			TestContext.WriteLine(Environment.NewLine);

		}


		[TestMethod]
		public void TestPrefixNotation()
		{
			string three =	  "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
			string fourteen = "5 + ((1 + 2) * 4) - 3";
			string twenty_a = "1 + 9 * 2 - 11 / 14 - 12 / 12";
			string twenty_b = "18 + 12 / 5 - 2 * 2 * 4 / 19";


			string prefix1 = PrefixNotation.Convert(three);
			string prefix2 = PrefixNotation.Convert(fourteen);
			string prefix3 = PrefixNotation.Convert(twenty_a);
			string prefix4 = PrefixNotation.Convert(twenty_b);

			int result1 = 0;
			int result2 = 0;
			int result3 = 0;
			int result4 = 0;

			TestContext.WriteLine(prefix1);
			TestContext.WriteLine(prefix2);
			TestContext.WriteLine(prefix3);
			TestContext.WriteLine(prefix4);
			TestContext.WriteLine(Environment.NewLine);
			TestContext.WriteLine(Environment.NewLine);

		}
	}
}
