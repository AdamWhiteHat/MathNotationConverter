using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;
using System.Linq.Expressions;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestEvaluation_ExpressionTree
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Evaluation")]
		[TestMethod]
		public void TestPoly()
		{
			string inputExpression = "144*x*y - 12*y + 12*x - 3218148";
			string expectingPostfix = "144 x * y * 12 y * 12 x * 3218148 - + -";
			string expecting = "0";

			string postfixExpression = PostfixNotation.Convert(inputExpression);
			Expression expression = ExpressionTree.Convert(postfixExpression);
			int result = ExpressionTree.Evaluate<int>(expression, new int[] { 151, 148 });

			TestContext.WriteLine($"Input: {inputExpression}");
			TestContext.WriteLine($"Parsed into postfix: {postfixExpression}");
			TestContext.WriteLine($"ExpressionTree: {expression}");
			TestContext.WriteLine("=>");
			TestContext.WriteLine($"Expecting value: {expecting}");
			TestContext.WriteLine($"Evaluated value: {result}");
			TestContext.WriteLine("");

			Assert.AreEqual(expectingPostfix, postfixExpression, "PostfixNotation.Convert");
			Assert.AreEqual(expecting, result.ToString(), "Evaluate");
		}

		[TestCategory("Evaluation")]
		[TestMethod]
		public void TestProduct()
		{
			string inputExpression = "(12*x+1)*(12*y-1)";
			string expectingPostfix = "12 x * 1 + 12 y * 1 - *";
			string expecting = "-3218147";

			string postfixExpression = PostfixNotation.Convert(inputExpression);
			Expression expression = ExpressionTree.Convert(postfixExpression);
			int result = ExpressionTree.Evaluate<int>(expression, new int[] { 151, 148 });

			TestContext.WriteLine($"Input: {inputExpression}");
			TestContext.WriteLine($"Parsed into postfix: {postfixExpression}");
			TestContext.WriteLine($"ExpressionTree: {expression}");
			TestContext.WriteLine("=>");
			TestContext.WriteLine($"Expecting value: {expecting}");
			TestContext.WriteLine($"Evaluated value: {result}");
			TestContext.WriteLine("");

			Assert.AreEqual(expectingPostfix, postfixExpression, "PostfixNotation.Convert");
			Assert.AreEqual(expecting, result.ToString(), "Evaluate");
		}

	}
}

