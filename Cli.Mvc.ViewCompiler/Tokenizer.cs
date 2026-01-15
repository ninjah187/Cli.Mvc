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
        ModelTypeDeclaration,
        Whitespace
    }

    public class Tokenizer
    {
        public IReadOnlyList<Token> Tokenize(string text)
        {
            //var words = text.Split([" "], StringSplitOptions.RemoveEmptyEntries);

            //var tokens = words.SelectMany(Classify).ToList();

            //return tokens;

            var tokens = new List<Token>();

            // var lines = text.Split(["\n", "\r\n"], StringSplitOptions.None);
            var lines = Regex.Split(text, "(\n|\r\n)");

            // foreach (var line in lines)
            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];

                if (line == "")
                {
                    // TODO: consider whether leave empty lines or not.
                    continue;
                }

                // var words = line.Split(' ');

                var words = Regex.Split(line, "( )");

                // foreach (var word in words)
                for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
                {
                    var word = words[wordIndex];

                    tokens.AddRange(Classify(word));

                    //if (wordIndex == words.Length - 1)
                    //{
                    //    // last element. do not add trailing space
                    //    continue;
                    //}

                    //tokens.Add(new Token(" ", TokenType.Whitespace));
                }

                //if (lineIndex == lines.Length - 1)
                //{
                //    // last line. do not add trailing line-break
                //    continue;
                //}

                //tokens.Add(new Token("\r\n", TokenType.Whitespace));
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
