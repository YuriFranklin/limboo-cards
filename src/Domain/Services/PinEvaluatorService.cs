namespace LimbooCards.Domain.Services
{
    using LimbooCards.Domain.Entities;
    using System.Text.RegularExpressions;
    using System.Reflection;
    using LimbooCards.Domain.Events;
    public class PinEvaluatorService
    {
        public static CardCategoryApplied? EvaluateCardPins(Subject subject, Planner planner, Guid cardId)
        {
            if (planner.PinRules == null || planner.PinRules.Count == 0)
                return null;

            var appliedCategories = new Dictionary<string, bool>();

            foreach (var rule in planner.PinRules)
            {
                if (EvaluateExpression(subject, planner, rule.Expression))
                {
                    var key = rule.PinColor.ToString();
                    appliedCategories.TryAdd(key, true);
                }
            }

            if (appliedCategories.Count == 0)
                return null;

            return new CardCategoryApplied(cardId, appliedCategories);
        }

        private static bool EvaluateExpression(Subject subject, Planner planner, string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Expression cannot be empty", nameof(expression));

            var tokens = Tokenize(expression);
            var rpn = ConvertToRPN(tokens);
            return EvaluateRPN(subject, planner, rpn);
        }

        private static List<string> Tokenize(string expression)
        {
            var pattern = @"(\()|(\))|(\bAND\b|\bOR\b|&&|\|\|)|('[^']*')|([^\s()]+)";
            var matches = Regex.Matches(expression, pattern, RegexOptions.IgnoreCase);
            return matches.Cast<Match>().Select(m => m.Value).Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
        }

        private static readonly Dictionary<string, int> OperatorPrecedence = new()
        {
            { "OR", 1 }, { "||", 1 }, { "AND", 2 }, { "&&", 2 }
        };

        private static List<string> ConvertToRPN(List<string> tokens)
        {
            var output = new List<string>();
            var operators = new Stack<string>();

            foreach (var token in tokens)
            {
                if (IsOperator(token))
                {
                    while (operators.Any() && IsOperator(operators.Peek()) &&
                           OperatorPrecedence[operators.Peek().ToUpper()] >= OperatorPrecedence[token.ToUpper()])
                        output.Add(operators.Pop());
                    operators.Push(token);
                }
                else if (token == "(") operators.Push(token);
                else if (token == ")")
                {
                    while (operators.Any() && operators.Peek() != "(") output.Add(operators.Pop());
                    if (!operators.Any() || operators.Pop() != "(")
                        throw new InvalidOperationException("Mismatched parentheses");
                }
                else output.Add(token);
            }

            while (operators.Any())
            {
                var op = operators.Pop();
                if (op == "(" || op == ")") throw new InvalidOperationException("Mismatched parentheses");
                output.Add(op);
            }

            return output;
        }

        private static bool IsOperator(string token) =>
            token.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
            token.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
            token.Equals("&&") || token.Equals("||");

        private static bool EvaluateRPN(Subject subject, Planner planner, List<string> rpn)
        {
            var stack = new Stack<bool>();

            for (int i = 0; i < rpn.Count; i++)
            {
                var token = rpn[i];

                if (IsOperator(token))
                {
                    if (stack.Count < 2)
                        throw new InvalidOperationException("Invalid expression.");

                    var right = stack.Pop();
                    var left = stack.Pop();

                    stack.Push(token.ToUpper() switch
                    {
                        "AND" => left && right,
                        "&&" => left && right,
                        "OR" => left || right,
                        "||" => left || right,
                        _ => throw new InvalidOperationException($"Unknown operator {token}")
                    });
                }
                else
                {
                    if (i + 2 >= rpn.Count)
                        throw new InvalidOperationException($"Incomplete condition near '{token}'");

                    string objProp = token;
                    string op = rpn[++i];
                    string value = rpn[++i];
                    value = value.Trim('\'');

                    stack.Push(EvaluateCondition(subject, planner, objProp, op, value));
                }
            }

            if (stack.Count != 1)
                throw new InvalidOperationException("Invalid expression evaluation");

            return stack.Pop();
        }

        private static bool EvaluateCondition(Subject subject, Planner planner, string objProp, string op, string value)
        {
            object rootObj = objProp.StartsWith("Subject.") ? (object)subject :
                             objProp.StartsWith("Planner.") ? (object)planner :
                             throw new InvalidOperationException($"Unsupported root object in {objProp}");

            var relativePath = objProp.Substring(objProp.IndexOf('.') + 1);
            var propValues = GetPropertyValues(rootObj, relativePath);

            foreach (var val in propValues)
            {
                bool result = op.ToLower() switch
                {
                    "contains" => val.Contains(value, StringComparison.OrdinalIgnoreCase),
                    "not_contains" => !val.Contains(value, StringComparison.OrdinalIgnoreCase),
                    "equals" => string.Equals(val, value, StringComparison.OrdinalIgnoreCase),
                    "==" => string.Equals(val, value, StringComparison.OrdinalIgnoreCase),
                    "!=" => !string.Equals(val, value, StringComparison.OrdinalIgnoreCase),
                    ">" => CompareNumericOrText(val, value) > 0,
                    ">=" => CompareNumericOrText(val, value) >= 0,
                    "<" => CompareNumericOrText(val, value) < 0,
                    "<=" => CompareNumericOrText(val, value) <= 0,
                    _ => throw new InvalidOperationException($"Operator {op} not supported")
                };

                if (result) return true;
            }

            return false;
        }

        private static IEnumerable<string> GetPropertyValues(object obj, string propertyPath)
        {
            string[] parts = propertyPath.Split('.');
            object? current = obj;

            for (int i = 0; i < parts.Length; i++)
            {
                if (current == null) yield break;

                var prop = current.GetType().GetProperty(parts[i], BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) throw new InvalidOperationException($"Property {parts[i]} not found on {current.GetType().Name}");

                current = prop.GetValue(current);

                if (current is System.Collections.IEnumerable enumerable && !(current is string))
                {
                    if (i < parts.Length - 1)
                    {
                        foreach (var item in enumerable)
                        {
                            var subPath = string.Join('.', parts[(i + 1)..]);
                            foreach (var val in GetPropertyValues(item!, subPath))
                                yield return val;
                        }
                        yield break;
                    }
                }
            }

            if (current != null)
                yield return current.ToString()!;
        }

        private static int CompareNumericOrText(string left, string right)
        {
            if (double.TryParse(left, out double leftNum) && double.TryParse(right, out double rightNum))
            {
                return leftNum.CompareTo(rightNum);
            }

            return string.Compare(left, right, StringComparison.OrdinalIgnoreCase);
        }
    }
}
