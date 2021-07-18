using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNotationConverter;

namespace MathNotationParserTests
{
	[TestClass]
	public class TestParsing_PrefixNotation
	{
		private TestContext testContextInstance;
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test001()
		{
			string three = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
			string fourteen = "5 + ((1 + 2) * 4) - 3";
			string twenty_a = "1 + 9 * 2 - 11 / 14 - 12 / 12";
			string twenty_b = "18 + 12 / 5 - 2 * 2 * 4 / 19";

			string prefix1 = PrefixNotation.Convert(three);
			string prefix2 = PrefixNotation.Convert(fourteen);
			string prefix3 = PrefixNotation.Convert(twenty_a);
			string prefix4 = PrefixNotation.Convert(twenty_b);

			int result1 = 0;
			int result2 = 0;
			int result3 = 0;
			int result4 = 0;

			TestContext.WriteLine($"{three   } => {prefix1}");
			TestContext.WriteLine($"{fourteen} => {prefix2}");
			TestContext.WriteLine($"{twenty_a} => {prefix3}");
			TestContext.WriteLine($"{twenty_b} => {prefix4}");

			TestContext.WriteLine(Environment.NewLine);
		}
	}
}

