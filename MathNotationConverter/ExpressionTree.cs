using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using MathNotationConverter.ExpressionVisitors;

namespace MathNotationConverter
{
	public class ExpressionTree
	{
		private static string AllowedCharacters = StaticStrings.Numbers + StaticStrings.Operators + StaticStrings.Variables + " ";

		public static Expression Convert(string postfixNotationString)
		{
			if (string.IsNullOrWhiteSpace(postfixNotationString))
			{
				throw new ArgumentException("Argument postfixNotationString must not be null, empty or whitespace.", "postfixNotationString");
			}

			Dictionary<char, ParameterExpression> variablesDictionary = new Dictionary<char, ParameterExpression>();

			Stack<Expression> stack = new Stack<Expression>();
			string sanitizedString = new string(postfixNotationString.Where(c => AllowedCharacters.Contains(c)).ToArray());
			List<string> enumerablePostfixTokens = sanitizedString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (string token in enumerablePostfixTokens)
			{
				if (token.Length < 1) throw new Exception("Token.Length is less than one.");

				int tokenValue = 0;
				bool parseSuccess = int.TryParse(token, out tokenValue);

				if (token.Length > 1) // Numbers > 10 will have a token length > 1
				{
					if (!StaticStrings.IsNumeric(token) || !parseSuccess)
					{
						throw new Exception("Operators and operands must be separated by a space.");
					}
					stack.Push(Expression.Constant(tokenValue));
				}
				else
				{
					char tokenChar = token[0];

					if (StaticStrings.Numbers.Contains(tokenChar) && parseSuccess)
					{
						stack.Push(Expression.Constant(tokenValue));
					}
					else if (StaticStrings.Variables.Contains(tokenChar))
					{
						ParameterExpression variable = null;
						if (variablesDictionary.ContainsKey(tokenChar))
						{
							variable = variablesDictionary[tokenChar];
						}
						else
						{
							variable = Expression.Parameter(typeof(int), tokenChar.ToString());
							variablesDictionary.Add(tokenChar, variable);
						}

						stack.Push(variable);
					}
					else if (StaticStrings.Operators.Contains(tokenChar))
					{
						// There must be two operands for the operator to operate on
						if (stack.Count < 2) throw new FormatException("The algebraic string has not sufficient values in the expression for the number of operators; There must be two operands for the operator to operate on.");

						Expression left = stack.Pop();
						Expression right = stack.Pop();
						Expression operation = null;

						// ^ token uses Math.Pow, which both gives and takes double, hence convert
						if (tokenChar == '^')
						{
							left = Expressions.ConvertIfNeeded(left, typeof(double));
							right = Expressions.ConvertIfNeeded(right, typeof(double));

						}
						else // Math.Pow returns a double, so we must check here for all other operators
						{
							left = Expressions.ConvertIfNeeded(left, typeof(int));
							right = Expressions.ConvertIfNeeded(right, typeof(int));
						}

						switch (tokenChar)
						{
							case '+': operation = Expression.AddChecked(left, right); break;
							case '-': operation = Expression.SubtractChecked(left, right); break;
							case '*': operation = Expression.MultiplyChecked(left, right); break;
							case '/': operation = Expression.Divide(left, right); break;
							case '^': operation = Expression.Power(left, right); break;
							default: throw new Exception(string.Format("Unrecognized character '{0}'.", tokenChar));
						}

						stack.Push(operation);
					}
					else throw new Exception(string.Format("Unrecognized character '{0}'.", tokenChar));
				}
			}
			if (stack.Count != 1) throw new Exception("The input has too many values for the number of operators.");

			Expression result = stack.Pop();
			return result;
		}

		public static T Evaluate<T>(Expression expression, IEnumerable<T> parameters)
		{
			Expression copy = expression;

			List<ParameterExpression> expressionParameters = Parameters.FindAll(copy);
			if (parameters.Count() != expressionParameters.Count)
			{
				throw new ArgumentException($"The number of parameters is wrong; Expression has {expressionParameters.Count} parameters. Supplied {parameters.Count()} parameters.");
			}

			LambdaExpression lambda = Expression.Lambda(copy, expressionParameters.ToArray());
			Delegate compiled = lambda.Compile();

			object[] objParams = parameters.Select(t => (object)t).ToArray();

			object result = compiled.DynamicInvoke(objParams);
			return (T)result;
		}
	}
}
