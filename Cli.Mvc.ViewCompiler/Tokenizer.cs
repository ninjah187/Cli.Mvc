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

            var position = 0;
            var lineNumber = 0;
            var column = 0;

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

                    tokens.AddRange(Classify(word, position, lineNumber, column));

                    column += word.Length;
                    position += word.Length;
                }

                if (line == "\n" || line == "\r\n")
                {
                    lineNumber++;
                    column = 0;
                }
            }

            return tokens;
        }

        IEnumerable<Token> Classify(string word, int position, int line, int column)
        {
            Token CreateToken(string value, TokenType type) => new(value, type, position, line, column);

            if (word == "@model")
            {
                yield return CreateToken(word, TokenType.ModelTypeDeclaration);
                yield break;
            }

            if (word == "@foreach")
            {
                yield return CreateToken(word, TokenType.ForEach);
                yield break;
            }

            if (word == "@if")
            {
                yield return CreateToken(word, TokenType.If);
                yield break;
            }

            if (word == "(")
            {
                yield return CreateToken(word, TokenType.LeftParenthesis);
                yield break;
            }

            if (word == ")")
            {
                yield return CreateToken(word, TokenType.RightParenthesis);
                yield break;
            }

            if (word == "{")
            {
                yield return CreateToken(word, TokenType.LeftBrace);
                yield break;
            }

            if (word == "}")
            {
                yield return CreateToken(word, TokenType.RightBrace);
                yield break;
            }

            if (word.StartsWith("@"))
            {
                yield return CreateToken(word, TokenType.Variable);
                yield break;
            }

            if (word == " ")
            {
                yield return CreateToken(word, TokenType.Whitespace);
                yield break;
            }

            if (word == "\n" || word == "\r\n")
            {
                yield return CreateToken(word, TokenType.Whitespace);
                yield break;
            }

            yield return CreateToken(word, TokenType.Text);
        }
    }
}
