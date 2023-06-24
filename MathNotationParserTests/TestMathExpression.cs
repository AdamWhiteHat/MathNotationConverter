using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MathNotationConverter;
using MathNotationConverter.Solver;
using MathNotationConverter.ExpressionVisitors;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Security.Cryptography;

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
		public void SetVariables_Zero()
		{
			string expecting = "0";

			object result = SetVariables(151, 148);

			Assert.AreEqual(expecting, result.ToString(), "SetVariables_Zero");
		}

		[TestCategory("Both")]
		[TestMethod]
		public void SetVariables_NonZero()
		{
			object result = SetVariables(150, 149);
		}

		private object SetVariables(int x, int y)
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
			Expression mathExpr_xSet = Parameters.SetValue(mathExpr.ParsedExpression, 'x', x);
			TestContext.WriteLine($"Updated Expression: {mathExpr_xSet}");
			TestContext.WriteLine("");

			TestContext.WriteLine("Setting variable y...");
			Expression mathExpr_xySet = Parameters.SetValue(mathExpr_xSet, 'y', y);
			TestContext.WriteLine($"Updated Expression: {mathExpr_xySet}");
			TestContext.WriteLine("");

			//TestContext.WriteLine("Evaluating...");
			//mathExpr.Evaluate();

			Expression reduced = mathExpr_xySet;

			//while(reduced.CanReduce)
			//{
			//	reduced = reduced.Reduce();
			//}

			LambdaExpression lambda = Expression.Lambda(reduced);
			Delegate compiled = lambda.Compile();
			object result = compiled.DynamicInvoke();

			TestContext.WriteLine("");
			TestContext.WriteLine($"Result: {result}");
			TestContext.WriteLine("");

			return result;
		}

		[TestCategory("Both")]
		[TestMethod]
		public void Eval_Zero()
		{
			string expecting = "0";

			object result = Eval(151, 148);

			Assert.AreEqual(expecting, result.ToString(), "Eval_Zero");
		}

		[TestCategory("Both")]
		[TestMethod]
		public void Eval_NonZero()
		{
			object result = Eval(152, 147);
		}

		private object Eval(int x, int y)
		{
			string inputExpression = "144*x*y - 12*y + 12*x - 3218148";

			Dictionary<char, int> variableValueAssignmentDictionary = new Dictionary<char, int>()
			{
				{ 'x', x},
				{ 'y', y }
			};

			MathExpression mathExpr = new MathExpression(inputExpression);

			TestContext.WriteLine($"Input Expression: {mathExpr.InputExpression}");
			TestContext.WriteLine($"Postfix Expression: {mathExpr.PostfixExpression}");
			TestContext.WriteLine($"Parsed Expression: {mathExpr.ParsedExpression}");
			TestContext.WriteLine($"Variables: {string.Join(", ", mathExpr.Variables.Select(v => v.ToString()))}");
			TestContext.WriteLine("");

			mathExpr['x'].SetValue(variableValueAssignmentDictionary['x']);
			mathExpr['y'].SetValue(variableValueAssignmentDictionary['y']);

			TestContext.WriteLine("Evaluating...");
			TestContext.WriteLine($"{mathExpr.Variables[0]}");
			TestContext.WriteLine($"{mathExpr.Variables[1]}");
			TestContext.WriteLine("");

			object result = mathExpr.Evaluate();
			TestContext.WriteLine($"Result: {result}");
			TestContext.WriteLine("");

			return result;
		}
	}
}
