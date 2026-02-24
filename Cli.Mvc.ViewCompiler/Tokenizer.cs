using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace Cli.Mvc.ViewCompiler
{
    public enum TokenType
    {
        Text,
        Variable, // Rename to expression?
        ModelTypeDeclaration, // Rename to directive. same for @using = UsingDirectiveNode
        Whitespace,
        ForEach,
        LeftParenthesis,
        RightParenthesis,
        LeftBrace,
        RightBrace,
        If
    }

    public class Tokenizer
    {
        public IReadOnlyList<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();

            var lines = Regex.Split(text, "(\n|\r\n)");

            var lineNumber = 0;

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];

                if (line == "")
                {
                    continue;
                }

                var words = Regex.Split(line, "( )").Where(word => word != "").ToArray();

                for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
                {
                    var word = words[wordIndex];

                    tokens.AddRange(Classify(word, lineNumber));
                }

                if (line == "\n" || line == "\r\n")
                {
                    lineNumber++;
                }
            }

            return tokens;
        }

        IEnumerable<Token> Classify(string word, int line)
        {
            if (word == "@model")
            {
                yield return new Token(word, TokenType.ModelTypeDeclaration, line);
                yield break;
            }

            if (word == "@foreach")
            {
                yield return new Token(word, TokenType.ForEach, line);
                yield break;
            }

            if (word == "@if")
            {
                yield return new Token(word, TokenType.If, line);
                yield break;
            }

            if (word == "(")
            {
                yield return new Token(word, TokenType.LeftParenthesis, line);
                yield break;
            }

            if (word == ")")
            {
                yield return new Token(word, TokenType.RightParenthesis, line);
                yield break;
            }

            if (word == "{")
            {
                yield return new Token(word, TokenType.LeftBrace, line);
                yield break;
            }

            if (word == "}")
            {
                yield return new Token(word, TokenType.RightBrace, line);
                yield break;
            }

            if (word.StartsWith("@"))
            {
                yield return new Token(word, TokenType.Variable, line);
                yield break;
            }

            if (word == " ")
            {
                yield return new Token(word, TokenType.Whitespace, line);
                yield break;
            }

            if (word == "\n" || word == "\r\n")
            {
                yield return new Token(word, TokenType.Whitespace, line);
                yield break;
            }

            yield return new Token(word, TokenType.Text, line);
        }
    }
}
