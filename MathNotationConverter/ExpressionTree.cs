using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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
						ParameterExpression variable = Expression.Parameter(typeof(int), tokenChar.ToString());
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
							left = ConvertExpressionType(left, typeof(double));
							right = ConvertExpressionType(right, typeof(double));

						}
						else // Math.Pow returns a double, so we must check here for all other operators
						{
							left = ConvertExpressionType(left, typeof(int));
							right = ConvertExpressionType(right, typeof(int));
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
			List<ParameterExpression> expressionParameters = GetParameters(expression);
			if (parameters.Count() != expressionParameters.Count)
			{
				throw new ArgumentException($"The number of parameters is wrong; Expression has {expressionParameters.Count} parameters. Supplied {parameters.Count()} parameters.");
			}

			var lambda = Expression.Lambda(expression, expressionParameters);
			Delegate compiled = lambda.Compile();

			object result = compiled.DynamicInvoke(parameters);
			return (T)result;
		}

		private static Expression ConvertExpressionType(Expression expression, Type type)
		{
			return (expression.Type != type) ? Expression.Convert(expression, type) : expression;
		}

		private static List<ParameterExpression> GetParameters(Expression expression)
		{
			List<ParameterExpression> result = new List<ParameterExpression>();

			ParameterExpression parameter = expression as ParameterExpression;
			if (parameter != null)
			{
				result.Add(parameter);
			}
			else
			{
				BinaryExpression binary = expression as BinaryExpression;
				if (binary != null)
				{
					Expression left = binary.Left;
					Expression right = binary.Right;

					result.AddRange(GetParameters(left));
					result.AddRange(GetParameters(right));
				}
			}

			return result;
		}
	}
}
