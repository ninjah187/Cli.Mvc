﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Cli.Mvc.Parsing
{
    public class Tokenizer
    {
        public IEnumerable<Token> Tokenize(string command)
        {
            var words = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var stack = new Stack<string>(words.Reverse());

            return Classify(stack);
        }

        IEnumerable<Token> Classify(Stack<string> input)
        {
            while (input.Any())
            {
                var current = input.Pop();

                if (current.StartsWith("--"))
                {
                    yield return new Token(TokenType.Option, current.Substring(2, current.Length - 2));
                    continue;
                }

                if (current.StartsWith("-"))
                {
                    var tokens = current.Substring(1, current.Length - 1)
                        .Select(c => c.ToString())
                        .Select(c => new Token(TokenType.Option, c));

                    foreach (var token in tokens)
                    {
                        yield return token;
                    }

                    continue;
                }

                if (current.StartsWith("\""))
                {
                    while (input.Any())
                    {
                        var next = input.Pop();

                        current += " " + next; // TODO: preserve whitespaces here. now we truncate multiple spaces into one

                        if (next.EndsWith("\""))
                        {
                            current = current.Substring(1, current.Length - 1);
                            current = current.Substring(0, current.Length - 1);
                            yield return new Token(TokenType.Word, current);
                            break;
                        }
                    }
                    
                    continue;
                }

                yield return new Token(TokenType.Word, current);
            }
        }
    }
}
