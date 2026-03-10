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
                    new("hello", TokenType.Text, 0, 0, 0), new(" ", TokenType.Whitespace, 5, 0, 5), new("world", TokenType.Text, 6, 0, 6)
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
                    new("hello", TokenType.Text, 0, 0, 0), new(" ", TokenType.Whitespace, 5, 0, 5),
                ]),
                new VariableNode(new("@Model.Name", TokenType.Variable, 6, 0, 6))
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
                        new("@model", TokenType.ModelTypeDeclaration, 0, 0, 0), new("Cli.Mvc.Examples.Razor.Models.Ninja", TokenType.Text, 7, 0, 7)
                    ),
                    // TODO: should this be single TextNode or rather should line break introduce new nodes to match tokens structure and real world case more ???
                    new TextNode([
                        new("\r\n", TokenType.Whitespace, 44, 1, 0),
                        new("Hello", TokenType.Text, 46, 2, 0),
                        new(" ", TokenType.Whitespace, 51, 2, 5)
                    ]),
                    new VariableNode(new("@Model.Name", TokenType.Variable, 52, 2, 6))
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
                        new("@model", TokenType.ModelTypeDeclaration, 0, 0, 0), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 7, 0, 7)
                    ),
                    new TextNode([
                        new("\r\n", TokenType.Whitespace, 50, 1, 0)
                    ]),
                    new ForeachNode(new("@foreach", TokenType.ForEach, 52, 2, 0), [new("(var", TokenType.Text, 61, 2, 9), new("ninja", TokenType.Text, 66, 2, 14), new("in", TokenType.Text, 72, 2, 20), new("Model)", TokenType.Text, 75, 2, 23)],
                        new BlockNode(new("{", TokenType.LeftBrace, 83, 3, 0), new("}", TokenType.RightBrace, 105, 5, 0), [
                            new TextNode([
                                new(" ", TokenType.Whitespace, 86, 4, 0), new(" ", TokenType.Whitespace, 87, 4, 1), new(" ", TokenType.Whitespace, 88, 4, 2), new(" ", TokenType.Whitespace, 89, 4, 3), new("-", TokenType.Text, 90, 4, 4), new(" ", TokenType.Whitespace, 91, 4, 5)
                            ]),
                            new VariableNode(new("@ninja.Name", TokenType.Variable, 92, 4, 6)),
                            new TextNode([new("\r\n", TokenType.Whitespace, 103, 4, 17)])
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
                        new("@model", TokenType.ModelTypeDeclaration, 0, 0, 0), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 7, 0, 7)
                    ),
                    new TextNode([
                        new("\r\n", TokenType.Whitespace, 50, 1, 0)
                    ]),
                    new IfNode(new("@if", TokenType.If, 52, 2, 0), [new("(Model.Count", TokenType.Text, 56, 2, 4), new("==", TokenType.Text, 69, 2, 17), new("0)", TokenType.Text, 72, 2, 20)],
                        new BlockNode(new("{", TokenType.LeftBrace, 76, 3, 0), new("}", TokenType.RightBrace, 100, 5, 0), [
                            new TextNode([
                                new(" ", TokenType.Whitespace, 79, 4, 0), new(" ", TokenType.Whitespace, 80, 4, 1), new(" ", TokenType.Whitespace, 81, 4, 2), new(" ", TokenType.Whitespace, 82, 4, 3), new("Nothing", TokenType.Text, 83, 4, 4), new(" ", TokenType.Whitespace, 90, 4, 11), new("to", TokenType.Text, 91, 4, 12), new(" ", TokenType.Whitespace, 93, 4, 14), new("show", TokenType.Text, 94, 4, 15), new("\r\n", TokenType.Whitespace, 98, 4, 19)
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
