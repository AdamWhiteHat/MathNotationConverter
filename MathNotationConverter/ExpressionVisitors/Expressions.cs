using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MathNotationConverter.ExpressionVisitors
{
	public static class Expressions
	{
		public static Expression ConvertIfNeeded(Expression expression, Type type)
		{
			if (expression.Type != type)
			{
				return Expression.Convert(expression, type);
			}
			return expression;
		}

		public static Expression Simplify(Expression input)
		{
			Expression changedExpression = input;
			Internal.Simplify expressionVisitor;

			do
			{
				expressionVisitor = new Internal.Simplify();
				changedExpression = expressionVisitor.Visit(changedExpression);
			}
			while (expressionVisitor.Success == true);

			return changedExpression;
		}

		#region Internal ExpressionVisitor Classes

		internal static class Internal
		{
			public class Simplify : ExpressionVisitor
			{
				public bool Success { get; private set; } = false;

				protected override Expression VisitBinary(BinaryExpression node)
				{
					if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.Constant)
					{
						Success = true;
						var lambda = Expression.Lambda(node);
						Delegate compiled = lambda.Compile();
						object result = compiled.DynamicInvoke();
						return Expression.Constant(result);
					}
					else
					{
						return base.VisitBinary(node);
					}
				}
			}
		}

		#endregion

	}
}
