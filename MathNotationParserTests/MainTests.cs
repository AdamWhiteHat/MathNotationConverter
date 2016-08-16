using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class MainTests
	{
		[TestMethod]
		public void TestShuntingYardAlgorithm()
		{
			string input = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
			string expecting = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
			string result = "";

			result = ShuntingYardAlgorithm.Convert(input);

			Assert.AreEqual(expecting, result);
		}

		[TestMethod]
		public void TestPostfixEvaluation()
		{
			string input = "3 4 2 * 1 5 - 2 3 ^ ^ / +";
			int expecting = 3;
			int result = 0;

			result = PostfixNotation.Evaluate(input);

			Assert.AreEqual(expecting, result);
		}

		[TestMethod]
		public void End2End()
		{
			string input = "5 + ((1 + 2) * 4) - 3";
			string postfix = "";
			string expectingPostfix = "5 1 2 + 4 * + 3 -";
			int result = 0;
			int expectingResult = 14;

			postfix = ShuntingYardAlgorithm.Convert(input);

			Assert.AreEqual(expectingPostfix, postfix);

			result = PostfixNotation.Evaluate(postfix);

			Assert.AreEqual(expectingResult, result);
		}
	}
}
