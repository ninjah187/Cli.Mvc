using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.ViewCompiler.Tests
{
    public class ParserIfTests
    {
        [Fact]
        public void CanParseIf_OnlyKeyword()
        {
            var text = """@if""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [], null)
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf_KeywordAndHeader()
        {
            var text = """@if (true)""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [new("(true)", TokenType.Text, 4, 0, 4)], null)
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf_KeywordAndIncompleteHeader()
        {
            var text = """@if (""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [new("(", TokenType.LeftParenthesis, 4, 0, 4)], null)
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf_KeywordAndIncompleteHeader_2()
        {
            var text = """@if (tr""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [new("(tr", TokenType.Text, 4, 0, 4)], null)
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf_KeywordHeaderAndIncompleteBlock()
        {
            var text = """@if (true) {""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [new("(true)", TokenType.Text, 4, 0, 4)], new BlockNode(new("{", TokenType.LeftBrace, 11, 0, 11), null, []))
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf_KeywordHeaderAndEmptyBlock()
        {
            var text = """@if (true) {}""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [new("(true)", TokenType.Text, 4, 0, 4)], new BlockNode(new("{", TokenType.LeftBrace, 11, 0, 11), new("}", TokenType.RightBrace, 12, 0, 12), []))
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf_KeywordHeaderAndEmptyBlockWithSpace()
        {
            var text = """@if (true) { }""";

            var expectedTree = new AbstractSyntaxTree([
                new IfNode(new("@if", TokenType.If, 0, 0, 0), [new("(true)", TokenType.Text, 4, 0, 4)], new BlockNode(new("{", TokenType.LeftBrace, 11, 0, 11), new("}", TokenType.RightBrace, 13, 0, 13), [new TextNode([new(" ", TokenType.Whitespace, 12, 0, 12)])]))
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }
    }
}
