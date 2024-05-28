using Cli.Mvc.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Parsing
{
    public class ParserTests
    {
        [Fact]
        public void ParsesSingleWord()
        {
            var command = "ninja";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(1, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);

            Assert.Equal(0, result.Options.Count);
        }

        [Fact]
        public void ParsesManyWords()
        {
            var command = "ninja list";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(2, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("list", result.Arguments[1]);

            Assert.Equal(0, result.Options.Count);
        }

        [Fact]
        public void ParsesManyWordsWithOption()
        {
            var command = "ninja list --verbose";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(2, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("list", result.Arguments[1]);

            Assert.Equal(1, result.Options.Count);
            Assert.True(result.Options.Exists("verbose"));
            Assert.Null(result.Options["verbose"]);
        }

        [Fact]
        public void ParsesManyWordsWithShorthandOption()
        {
            var command = "ninja list -v";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(2, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("list", result.Arguments[1]);

            Assert.Equal(1, result.Options.Count);
            Assert.True(result.Options.Exists("v"));
            Assert.Null(result.Options["v"]);
        }

        [Fact]
        public void ParsesManyWordsWithMultipleShorthandOptions()
        {
            var command = "ninja list -als";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(2, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("list", result.Arguments[1]);
            
            Assert.Equal(3, result.Options.Count);
            Assert.True(result.Options.Exists("a"));
            Assert.Null(result.Options["a"]);
            Assert.True(result.Options.Exists("l"));
            Assert.Null(result.Options["l"]);
            Assert.True(result.Options.Exists("s"));
            Assert.Null(result.Options["s"]);
        }

        [Fact]
        public void ParsesManyWordsWithVerboseOptionWithValue()
        {
            var command = "ninja list --src .";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(2, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("list", result.Arguments[1]);

            Assert.Equal(1, result.Options.Count);
            Assert.True(result.Options.Exists("src"));
            Assert.Equal(".", result.Options["src"]);
        }

        [Fact]
        public void ParsesManyWordsWithQuotemarks()
        {
            var command = "ninja add \"Bob Charles Bobbington\"";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("add", result.Arguments[1]);
            Assert.Equal("Bob Charles Bobbington", result.Arguments[2]);
        }

        [Fact]
        public void ParsesManyWordsWithSingleWordInQuotemarks()
        {
            var command = "ninja add \"Bob\"";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("add", result.Arguments[1]);
            Assert.Equal("Bob", result.Arguments[2]);
        }

        [Fact]
        public void ParsesManyWordsWithVerboseOptionWithValueInsideQuotemarks()
        {
            var command = "ninja add \"Bob Charles Bobbington\" --weapon \"The Sharpest Katana\"";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("add", result.Arguments[1]);
            Assert.Equal("Bob Charles Bobbington", result.Arguments[2]);
            Assert.Equal("The Sharpest Katana", result.Options["weapon"]);
        }

        [Fact]
        public void ParsesManyWordsWithVerboseOptionWithMultipleWordsValue()
        {
            var command = "ninja add --name Bob Charles Bobbington";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("add", result.Arguments[1]);
            Assert.Equal("Bob Charles Bobbington", result.Options["name"]);
        }

        [Fact]
        public void ParsesMultipleOptionsWithValuesWithoutQuotemarks()
        {
            var command = "ninja add Bob --weapon The Sharpest Katana --hp 100";
            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("add", result.Arguments[1]);
            Assert.Equal("Bob", result.Arguments[2]);
            Assert.Equal("The Sharpest Katana", result.Options["weapon"]);
            Assert.Equal("100", result.Options["hp"]);
        }

        [Fact]
        public void ParsesSingleWordArgumentAndMultipleOptionsInsideQuotemarks()
        {
            var command = "ninja add \"Bob\" --weapon \"The Sharpest Katana\" --hp \"100\"";

            var parser = new Parser(command);

            var result = parser.Parse();

            Assert.Equal(3, result.Arguments.Count);
            Assert.Equal("ninja", result.Arguments[0]);
            Assert.Equal("add", result.Arguments[1]);
            Assert.Equal("Bob", result.Arguments[2]);

            Assert.Equal(2, result.Options.Count);
            Assert.Equal("The Sharpest Katana", result.Options["weapon"]);
            Assert.Equal("100", result.Options["hp"]);
        }
    }
}
