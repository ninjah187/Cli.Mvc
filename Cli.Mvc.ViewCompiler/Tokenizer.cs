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
        RightBrace
    }

    public class Tokenizer
    {
        public IReadOnlyList<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();

            var lines = Regex.Split(text, "(\n|\r\n)");

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

                    tokens.AddRange(Classify(word));
                }
            }

            return tokens;
        }

        IEnumerable<Token> Classify(string word)
        {
            if (word == "@model")
            {
                yield return new Token(word, TokenType.ModelTypeDeclaration);
                yield break;
            }

            if (word == "@foreach")
            {
                yield return new Token(word, TokenType.ForEach);
                yield break;
            }

            if (word == "(")
            {
                yield return new Token(word, TokenType.LeftParenthesis);
                yield break;
            }

            if (word == ")")
            {
                yield return new Token(word, TokenType.RightParenthesis);
                yield break;
            }

            if (word == "{")
            {
                yield return new Token(word, TokenType.LeftBrace);
                yield break;
            }

            if (word == "}")
            {
                yield return new Token(word, TokenType.RightBrace);
                yield break;
            }

            if (word.StartsWith("@"))
            {
                yield return new Token(word, TokenType.Variable);
                yield break;
            }

            if (word == " ")
            {
                yield return new Token(word, TokenType.Whitespace);
                yield break;
            }

            if (word == "\n" || word == "\r\n")
            {
                yield return new Token(word, TokenType.Whitespace);
                yield break;
            }

            yield return new Token(word, TokenType.Text);
        }

        //TokenType Classify(string word)
        //{
        //    if (word == "@model")
        //    {
        //        return TokenType.ModelTypeDeclaration;
        //    }

        //    if (word.StartsWith("@"))
        //    {
        //        return TokenType.Variable;
        //    }

        //    return TokenType.Text;
        //}
    }
}
