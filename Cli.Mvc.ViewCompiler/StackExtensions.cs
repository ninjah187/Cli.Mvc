using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public static class StackExtensions
    {
        public static IReadOnlyList<T> PopWhile<T>(this Stack<T> stack, Func<T, bool> condition)
        {
            var result = new List<T>();

            while (stack.Count > 0)
            {
                var item = stack.Peek();

                if (condition(item))
                {
                    result.Add(stack.Pop());
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        public static IEnumerable<T> PopUntil<T>(this Stack<T> stack, Func<T, bool> condition)
        {
            while (stack.Count > 0)
            {
                var item = stack.Pop();

                yield return item;

                if (condition(item))
                {
                    break;
                }
            }
        }
    }
}
