using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MathNotationConverter;
using MathNotationConverter.Solver;
using MathNotationConverter.ExpressionVisitors;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestMathExpression
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Both")]
		[TestMethod]
		public void TestMathExp_SetVariable()
		{
			string inputExpression = "144*x*y - 12*y + 12*x - 3218148";

			MathExpression mathExpr = new MathExpression(inputExpression);

			TestContext.WriteLine($"Input Expression: {mathExpr.InputExpression}");
			TestContext.WriteLine($"Postfix Expression: {mathExpr.PostfixExpression}");
			TestContext.WriteLine($"Parsed Expression: {mathExpr.ParsedExpression}");
			TestContext.WriteLine($"Variables: {string.Join(", ", mathExpr.Variables.Select(v => v.ToString()))}");
			TestContext.WriteLine("");

			MathExpression copy = mathExpr.Clone();

			TestContext.WriteLine("Setting variable x...");
			Parameters.SetValue(mathExpr.ParsedExpression, 'x', 151);
			TestContext.WriteLine($"Updated Expression: {mathExpr.ParsedExpression}");
			TestContext.WriteLine("");

			TestContext.WriteLine("Setting variable y...");
			Parameters.SetValue(mathExpr.ParsedExpression, 'y', 148);
			TestContext.WriteLine($"Updated Expression: {copy.ParsedExpression}");
			TestContext.WriteLine("");

			//TestContext.WriteLine("Evaluating...");
			//mathExpr.Evaluate();

			//TestContext.WriteLine("");
			//TestContext.WriteLine($"Result: {mathExpr.Value}");
			//TestContext.WriteLine("");
		}

		[TestCategory("Both")]
		[TestMethod]
		public void TestMathExp_Eval()
		{
			string inputExpression = "144*x*y - 12*y + 12*x - 3218148";
			string expecting = "0";

			MathExpression mathExpr = new MathExpression(inputExpression);

			TestContext.WriteLine($"Input Expression: {mathExpr.InputExpression}");
			TestContext.WriteLine($"Postfix Expression: {mathExpr.PostfixExpression}");
			TestContext.WriteLine($"Parsed Expression: {mathExpr.ParsedExpression}");
			TestContext.WriteLine($"Variables: {string.Join(", ", mathExpr.Variables.Select(v => v.ToString()))}");
			TestContext.WriteLine("");

			TestContext.WriteLine("Evaluating...");
			object result = mathExpr.Evaluate(new int[] { 151, 148 });

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result: {mathExpr.Value}");
			TestContext.WriteLine("");

			Assert.AreEqual(expecting, result.ToString(), "Evaluate");
		}
	}
}
