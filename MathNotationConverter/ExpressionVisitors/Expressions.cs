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
	}
}
