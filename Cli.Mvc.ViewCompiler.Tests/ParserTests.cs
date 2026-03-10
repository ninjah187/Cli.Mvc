using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.ViewCompiler.Tests
{
    public class ParserTests
    {
        [Fact]
        public void CanParseHelloWorld()
        {
            var text = "hello world";

            var expectedTree = new AbstractSyntaxTree([
                new TextNode([
                    new("hello", TokenType.Text, 0, 0), new(" ", TokenType.Whitespace, 0, 5), new("world", TokenType.Text, 0, 6)
                ]),
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree);
        }

        [Fact]
        public void CanParseTextWithVariable()
        {
            var text = "hello @Model.Name";

            var expectedTree = new AbstractSyntaxTree([
                new TextNode([
                    new("hello", TokenType.Text, 0, 0), new(" ", TokenType.Whitespace, 0, 5),
                ]),
                new VariableNode(new("@Model.Name", TokenType.Variable, 0, 6))
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree);
        }

        [Fact]
        public void CanParseComplexCase()
        {
            var text = """
                @model Cli.Mvc.Examples.Razor.Models.Ninja

                Hello @Model.Name
                """;

            var expectedTree = new AbstractSyntaxTree([
                    new ModelTypeDeclarationNode(
                        new("@model", TokenType.ModelTypeDeclaration,  0, 0), new("Cli.Mvc.Examples.Razor.Models.Ninja", TokenType.Text, 0, 7)
                    ),
                    // TODO: should this be single TextNode or rather should line break introduce new nodes to match tokens structure and real world case more ???
                    new TextNode([
                        new("\r\n", TokenType.Whitespace, 0, 42),
                        new("\r\n", TokenType.Whitespace, 1, 0),
                        new("Hello", TokenType.Text, 2, 0),
                        new(" ", TokenType.Whitespace, 2, 5)
                    ]),
                    new VariableNode(new("@Model.Name", TokenType.Variable, 2, 6))
                ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseForeach()
        {
            var text = """
                @model List<Cli.Mvc.Examples.Razor.Models.Ninja>

                @foreach (var ninja in Model)
                {
                    - @ninja.Name
                }
                """;

            var expectedTree = new AbstractSyntaxTree([
                    new ModelTypeDeclarationNode(
                        new("@model", TokenType.ModelTypeDeclaration,  0, 0), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 0, 7)
                    ),
                    new TextNode([
                        new("\r\n", TokenType.Whitespace, 1, 0)
                    ]),
                    new ForeachNode(new("@foreach", TokenType.ForEach, 2, 0), [new("(var", TokenType.Text, 2, 9), new("ninja", TokenType.Text, 2, 14), new("in", TokenType.Text, 2, 20), new("Model)", TokenType.Text, 2, 23)],
                        new BlockNode(new("{", TokenType.LeftBrace, 3, 0), new("}", TokenType.RightBrace, 5, 0), [
                            new TextNode([
                                new(" ", TokenType.Whitespace, 4, 0), new(" ", TokenType.Whitespace, 4, 1), new(" ", TokenType.Whitespace, 4, 2), new(" ", TokenType.Whitespace, 4, 3), new("-", TokenType.Text, 4, 4), new(" ", TokenType.Whitespace, 4, 5)
                            ]),
                            new VariableNode(new("@ninja.Name", TokenType.Variable, 4, 6)),
                            new TextNode([new("\r\n", TokenType.Whitespace, 4, 17)])
                        ])
                    )
                ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }

        [Fact]
        public void CanParseIf()
        {
            var text = """
                @model List<Cli.Mvc.Examples.Razor.Models.Ninja>

                @if (Model.Count == 0)
                {
                    Nothing to show
                }
                """;

            var expectedTree = new AbstractSyntaxTree([
                    new ModelTypeDeclarationNode(
                        new("@model", TokenType.ModelTypeDeclaration,  0, 0), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 0, 7)
                    ),
                    new TextNode([
                        new("\r\n", TokenType.Whitespace, 1, 0)
                    ]),
                    new IfNode(new("@if", TokenType.If, 2, 0), [new("(Model.Count", TokenType.Text, 2, 4), new("==", TokenType.Text, 2, 17), new("0)", TokenType.Text, 2, 20)],
                        new BlockNode(new("{", TokenType.LeftBrace, 3, 0), new("}", TokenType.RightBrace, 5, 0), [
                            new TextNode([
                                new(" ", TokenType.Whitespace, 4, 0), new(" ", TokenType.Whitespace, 4, 1), new(" ", TokenType.Whitespace, 4, 2), new(" ", TokenType.Whitespace, 4, 3), new("Nothing", TokenType.Text, 4, 4), new(" ", TokenType.Whitespace, 4, 11), new("to", TokenType.Text, 4, 12), new(" ", TokenType.Whitespace, 4, 14), new("show", TokenType.Text, 4, 15), new("\r\n", TokenType.Whitespace, 4, 19)
                            ])
                        ])
                    )
                ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }
    }
}
