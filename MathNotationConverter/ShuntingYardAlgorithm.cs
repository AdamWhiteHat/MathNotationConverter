using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathNotationConverter
{
	public static class ShuntingYardAlgorithm
	{
		private static void AddToOutput(List<char> output, params char[] chars)
		{
			if (chars.Length < 1) throw new ArgumentOutOfRangeException();

			output.AddRange(chars);
			output.Add(' ');
		}

		public static string Convert(string infixNotationString)
		{
			if (string.IsNullOrWhiteSpace(infixNotationString)) throw new ArgumentException("Argument infixNotationString must not be null, empty or whitespace.", "infixNotationString");

			string number = string.Empty;
			List<string> enumerableInfixTokens = new List<string>();
			string inputString = new string(infixNotationString.Where(c => StaticStrings.AllowedCharacters.Contains(c)).ToArray());
			foreach (char c in inputString)
			{
				if (StaticStrings.Operators.Contains(c) || "()".Contains(c))
				{
					if (number.Length > 0)
					{
						enumerableInfixTokens.Add(number);
						number = string.Empty;
					}
					enumerableInfixTokens.Add(c.ToString());
				}
				else if (StaticStrings.Numbers.Contains(c))
				{
					number += c.ToString();
				}
				else throw new Exception(string.Format("Unexpected character '{0}'.", c));
			}

			if (number.Length > 0)
			{
				enumerableInfixTokens.Add(number);
				number = string.Empty;
			}

			List<char> outputQueue = new List<char>();
			Stack<char> operatorStack = new Stack<char>();
			foreach (string token in enumerableInfixTokens)
			{
				if (StaticStrings.IsNumeric(token))
				{
					AddToOutput(outputQueue, token.ToArray());
				}
				else if (token.Length == 1)
				{
					char c = token[0];

					if (StaticStrings.Numbers.Contains(c))
					{
						AddToOutput(outputQueue, c);
					}
					else if (StaticStrings.Operators.Contains(c))
					{
						if (operatorStack.Count > 0)
						{
							char o = operatorStack.Peek();
							if (
									(StaticStrings.AssociativityDictionary[c] == StaticStrings.Associativity.Left
											&& StaticStrings.PrecedenceDictionary[c] <= StaticStrings.PrecedenceDictionary[o])
									||
									(StaticStrings.AssociativityDictionary[c] == StaticStrings.Associativity.Right
											&& StaticStrings.PrecedenceDictionary[c] < StaticStrings.PrecedenceDictionary[o])
								)
							{
								AddToOutput(outputQueue, operatorStack.Pop());
							}
						}
						operatorStack.Push(c);
					}
					else if (c == '(')
					{
						operatorStack.Push(c);
					}
					else if (c == ')')
					{
						bool leftParenthesisFound = false;
						while (operatorStack.Count > 0)
						{
							char o = operatorStack.Peek();
							if (o == '(')
							{
								operatorStack.Pop();
								leftParenthesisFound = true;
								break;
							}
							AddToOutput(outputQueue, operatorStack.Pop());
						}

						if (!leftParenthesisFound) throw new FormatException("The algebraic string contains mismatched parentheses (missing a left parenthesis).");
					}
					else throw new Exception("Unrecognized character " + c.ToString());
				}
				else throw new Exception(token + " is not numeric or has a length greater than 1.");
			} // end foreach

			while (operatorStack.Count > 0)
			{
				char o = operatorStack.Pop();
				if ("()".Contains(o)) throw new FormatException("The algebraic string contains mismatched parentheses (extra " + (o == '(' ? "left" : "right") + " parenthesis).");

				AddToOutput(outputQueue, o);
			}

			return new string(outputQueue.ToArray()).TrimEnd();
		}
	}
}
