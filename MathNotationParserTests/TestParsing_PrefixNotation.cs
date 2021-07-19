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
			string prefixResult1 = PrefixNotation.Convert(three);
			string expectingPrefix1 = "3 4 2 * 1 5 - 2 3 ^ ^ / +";

			TestContext.WriteLine($"{three   } => {prefixResult1}");
			TestContext.WriteLine(Environment.NewLine);

			Assert.AreEqual(expectingPrefix1, prefixResult1, "#1");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test002()
		{
			string fourteen = "5 + ((1 + 2) * 4) - 3";
			string prefixResult2 = PrefixNotation.Convert(fourteen);
			string expectingPrefix2 = "5 1 2 + 4 * + 3 -";

			TestContext.WriteLine($"{fourteen} => {prefixResult2}");
			TestContext.WriteLine(Environment.NewLine);

			Assert.AreEqual(expectingPrefix2, prefixResult2, "#2");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test003()
		{
			string twenty_a = "1 + 9 * 2 - 11 / 14 - 12 / 12";
			string prefixResult3 = PrefixNotation.Convert(twenty_a);
			string expectingPrefix3 = "1 9 2 * 11 14 / 12 12 / - - +";

			TestContext.WriteLine($"{twenty_a} => {prefixResult3}");
			TestContext.WriteLine(Environment.NewLine);

			Assert.AreEqual(expectingPrefix3, prefixResult3, "#3");
		}

		[TestCategory("Parsing")]
		[TestMethod]
		public void Test004()
		{
			string twenty_b = "18 + 12 / 5 - 2 * 2 * 4 / 19";
			string prefixResult4 = PrefixNotation.Convert(twenty_b);
			string expectingPrefix4 = "18 12 5 / 2 2 * 4 * 19 / - +";

			TestContext.WriteLine($"{twenty_b} => {prefixResult4}");
			TestContext.WriteLine(Environment.NewLine);

			Assert.AreEqual(expectingPrefix4, prefixResult4, "#4");
		}
	}
}

