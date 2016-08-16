using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNotationConverter
{
	public static class StaticStrings
	{
		public static string Numbers = "0123456789";
		public static string Operators = "+-*/^";

		public static bool IsNumeric(string text)
		{
			return string.IsNullOrWhiteSpace(text) ? false : text.All(c => Numbers.Contains(c));
		}
	}
}
