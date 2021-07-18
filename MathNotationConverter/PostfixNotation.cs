using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MathNotationConverter
{
	public static class PostfixNotation
	{
		public static string Convert(string infixNotationString)
		{
			if (string.IsNullOrWhiteSpace(infixNotationString))
			{
				throw new ArgumentException("Argument infixNotationString must not be null, empty or whitespace.", "infixNotationString");
			}

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
				else if (StaticStrings.IsVariable(c))
				{
					enumerableInfixTokens.Add(c.ToString());
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
					else if (StaticStrings.IsVariable(c))
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

		private static void AddToOutput(List<char> output, params char[] chars)
		{
			if (chars.Length < 1) throw new ArgumentOutOfRangeException();

			output.AddRange(chars);
			output.Add(' ');
		}

		public static int Evaluate(string postfixNotationString)
		{
			if (string.IsNullOrWhiteSpace(postfixNotationString)) throw new ArgumentException("Argument postfixNotationString must not be null, empty or whitespace.", "postfixNotationString");

			Stack<string> stack = new Stack<string>();
			string sanitizedString = new string(postfixNotationString.Where(c => StaticStrings.AllowedCharacters.Contains(c) || c == ' ').ToArray());
			List<string> enumerablePostfixTokens = sanitizedString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (string token in enumerablePostfixTokens)
			{
				if (token.Length > 0)
				{
					if (token.Length > 1)
					{
						if (!StaticStrings.IsNumeric(token)) throw new Exception("Operators and operands must be separated by a space.");
						stack.Push(token);
					}
					else
					{
						char tokenChar = token[0];

						if (StaticStrings.Numbers.Contains(tokenChar))
						{
							stack.Push(tokenChar.ToString());
						}
						else if (StaticStrings.Operators.Contains(tokenChar))
						{
							if (stack.Count < 2) throw new FormatException("The algebraic string has not sufficient values in the expression for the number of operators.");

							string r = stack.Pop();
							string l = stack.Pop();

							int rhs = int.MinValue;
							int lhs = int.MinValue;

							bool parseSuccess = int.TryParse(r, out rhs);
							parseSuccess &= int.TryParse(l, out lhs);
							parseSuccess &= (rhs != int.MinValue && lhs != int.MinValue);

							if (!parseSuccess) throw new Exception("Unable to parse valueStack characters to Int32.");

							int value = int.MinValue;
							if (tokenChar == '+')
							{
								value = lhs + rhs;
							}
							else if (tokenChar == '-')
							{
								value = lhs - rhs;
							}
							else if (tokenChar == '*')
							{
								value = lhs * rhs;
							}
							else if (tokenChar == '/')
							{
								value = lhs / rhs;
							}
							else if (tokenChar == '^')
							{
								value = (int)Math.Pow(lhs, rhs);
							}

							if (value != int.MinValue)
							{
								stack.Push(value.ToString());
							}
							else throw new Exception("Value never got set.");
						}
						else throw new Exception(string.Format("Unrecognized character '{0}'.", tokenChar));
					}
				}
				else throw new Exception("Token length is less than one.");
			}

			if (stack.Count == 1)
			{
				int result = 0;
				if (!int.TryParse(stack.Pop(), out result)) throw new Exception("Last value on stack could not be parsed into an integer.");
				return result;
			}
			else throw new Exception("The input has too many values for the number of operators.");

		} // method
	} // class
} // namespace
