using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MathNotationConverter.ExpressionVisitors
{
	public static class Parameters
	{
		public static List<ParameterExpression> GetDistinct(Expression expression)
		{
			var expressionVisitor = new Internal.GetDistinctInstances();
			expressionVisitor.Visit(expression);

			return expressionVisitor.FoundParameters.ToList();
		}

		public static List<ParameterExpression> SetUniqueInstances(ref Expression expression)
		{
			var expressionVisitor = new Internal.SetSingletonInstances();
			var changedExpression = expressionVisitor.Visit(expression);
			expression = changedExpression;

			return expressionVisitor.FoundParameters.ToList();
		}

		public static Expression SetValue(Expression expression, char symbol, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			Internal.SetValue expressionVisitor = new Internal.SetValue(symbol, value);
			var changedExpression = expressionVisitor.Visit(expression);

			return changedExpression;
		}

		#region Internal ExpressionVisitor Classes

		internal static class Internal
		{
			public class GetDistinctInstances : ExpressionVisitor
			{
				public List<ParameterExpression> FoundParameters = new List<ParameterExpression>();

				protected override Expression VisitParameter(ParameterExpression node)
				{
					ParameterExpression match = FoundParameters.Where(pe => pe.Name == node.Name).FirstOrDefault();
					if (match == default(ParameterExpression))
					{
						FoundParameters.Add(node);
					}
					return base.VisitParameter(node);
				}
			}

			public class SetSingletonInstances : ExpressionVisitor
			{
				public List<ParameterExpression> FoundParameters = new List<ParameterExpression>();

				protected override Expression VisitParameter(ParameterExpression node)
				{
					ParameterExpression match = FoundParameters.Where(pe => pe.Name == node.Name).FirstOrDefault();
					if (match == default(ParameterExpression))
					{
						FoundParameters.Add(node);
						return base.VisitParameter(node);
					}
					else
					{
						return match;
					}
				}
			}

			public class SetValue : ExpressionVisitor
			{
				private readonly object _value;
				private readonly string _name;

				public SetValue(char symbol, object value)
				{
					this._name = symbol.ToString();
					this._value = value;
				}

				protected override Expression VisitParameter(ParameterExpression node)
				{
					if (node.Name == _name)
					{
						return Expression.Constant(_value);
					}
					return base.VisitParameter(node);
				}
			}
		}

		#endregion

	}
}
