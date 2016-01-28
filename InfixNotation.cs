using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNotationConverter
{
	public static class InfixNotation
	{
		public static int Evaluate(string infixNotationString)
		{
			string postFixNotationString = ShuntingYardAlgorithm.Convert(infixNotationString);
			return PostfixNotation.Evaluate(postFixNotationString);
		}
	}
}
