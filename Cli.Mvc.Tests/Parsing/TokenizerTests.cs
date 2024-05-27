using Cli.Mvc.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Parsing
{
    public class TokenizerTests
    {
        [Fact]
        public void TokenizesSingleWord()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja")
            };

            Assert.Equal(expected, tokens);
        }

        [Fact]
        public void TokenizesManyWords()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja list").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja"),
                new(TokenType.Word, "list")
            };

            Assert.Equal(expected, tokens);
        }

        [Fact]
        public void TokenizesManyWordsWithOption()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja list --verbose").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja"),
                new(TokenType.Word, "list"),
                new(TokenType.Option, "verbose")
            };

            Assert.Equal(expected, tokens);
        }

        [Fact]
        public void TokenizesManyWordsWithShorthandOption()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja list -v").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja"),
                new(TokenType.Word, "list"),
                new(TokenType.Option, "v")
            };

            Assert.Equal(expected, tokens);
        }

        [Fact]
        public void TokenizesManyWordsWithMultipleShorthandOptions()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja list -als").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja"),
                new(TokenType.Word, "list"),
                new(TokenType.Option, "a"),
                new(TokenType.Option, "l"),
                new(TokenType.Option, "s")
            };

            Assert.Equal(expected, tokens);
        }

        [Fact]
        public void TokenizesManyWordsWithVerboseOptionWithValue()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja list --src .").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja"),
                new(TokenType.Word, "list"),
                new(TokenType.Option, "src"),
                new(TokenType.Word, ".")
            };

            Assert.Equal(expected, tokens);
        }

        [Fact]
        public void TokenizesManyWordsWithQuotemarks()
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.Tokenize("ninja add \"Bob Charles Bobbington\"").ToList();

            var expected = new List<Token>
            {
                new(TokenType.Word, "ninja"),
                new(TokenType.Word, "add"),
                new(TokenType.Word, "Bob Charles Bobbington"),
            };

            Assert.Equal(expected, tokens);
        }
    }
}
