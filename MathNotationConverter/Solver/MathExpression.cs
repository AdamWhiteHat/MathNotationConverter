using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MathNotationConverter.ExpressionVisitors;

namespace MathNotationConverter.Solver
{
	public class MathExpression
	{
		public string InputExpression { get; private set; }
		public string PostfixExpression { get; private set; }
		public Expression ParsedExpression { get; private set; }
		public List<Variable> Variables { get; private set; }

		public Variable this[char c]
		{
			get { return Variables.FirstOrDefault(v => char.ToUpperInvariant(v.Symbol).Equals(char.ToUpperInvariant(c))); }
		}

		public object Value { get; private set; }

		private MathExpression()
		{ }

		public MathExpression(string expression)
		{
			InputExpression = expression;
			PostfixExpression = PostfixNotation.Convert(InputExpression);
			Expression tempExpression = ExpressionTree.Convert(PostfixExpression);

			List<ParameterExpression> parameters = Parameters.FindAll(tempExpression);

			ParsedExpression = tempExpression;
			List<Variable> variables = new List<Variable>();
			foreach (ParameterExpression parameter in parameters)
			{
				variables.Add(new Variable(parameter));
			}
			Variables = variables;
		}

		public MathExpression Clone()
		{
			return new MathExpression()
			{
				InputExpression = this.InputExpression,
				PostfixExpression = this.PostfixExpression,
				ParsedExpression = this.ParsedExpression,
				Value = this.Value,
				Variables = this.Variables.Select(v => v.Clone()).ToList()
			};
		}

		public object Evaluate()
		{
			return Evaluate(Variables.Select(v => v.Value).ToArray());
		}

		public T Evaluate<T>(IEnumerable<T> args)
		{
			ParameterExpression[] parameters = Variables.Select(v => v.ParameterExpression).ToArray();
			LambdaExpression lambda = Expression.Lambda(ParsedExpression, parameters);
			Delegate compiled = lambda.Compile();

			object[] objParams = args.Select(t => (object)t).ToArray();
			Value = compiled.DynamicInvoke(objParams);
			return (T)Value;
		}
	}
}
