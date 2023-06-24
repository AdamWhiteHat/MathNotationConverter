using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MathNotationConverter.Solver
{
	public class Variable
	{
		public char Symbol { get; private set; }
		public Type Type { get; private set; }
		public object Value { get; private set; }
		public ParameterExpression ParameterExpression { get; private set; }

		private Variable()
		{ }

		public Variable(ParameterExpression parameter)
		{
			ParameterExpression = parameter;

			this.Type = ParameterExpression.Type;
			Symbol = ParameterExpression.Name.Single();
			Value = null;
		}

		public void SetValue(object value)
		{
			if (Value == null || Value != value)
			{
				Value = value;
			}
		}

		public Variable Clone()
		{
			return new Variable()
			{
				Symbol = this.Symbol,
				Type = this.Type,
				Value = this.Value,
				ParameterExpression = Expression.Parameter(ParameterExpression.Type, ParameterExpression.Name)
			};
		}

		public override string ToString()
		{
			if (Value == null)
			{
				return Symbol.ToString();
			}
			else
			{
				return $"{Symbol} = {Value}";
			}
		}
	}
}
