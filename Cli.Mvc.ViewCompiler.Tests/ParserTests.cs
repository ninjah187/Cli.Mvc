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
        public void CanParseSimpleText()
        {
            var text = "hello world";

            var expectedTree = new AbstractSyntaxTree([
                new TextNode("hello world"),
            ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree);
        }

        [Fact]
        public void CanParseTextWithVariable()
        {
            var text = "hello @Name";

            var expectedTree = new AbstractSyntaxTree([
                new TextNode("hello "),
                new VariableNode("@Name")
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
                    new ModelTypeDeclarationNode("@model", "Cli.Mvc.Examples.Razor.Models.Ninja"),
                    new TextNode("\r\n\r\nHello "),
                    new VariableNode("@Model.Name")
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
                    new ModelTypeDeclarationNode("@model", "List<Cli.Mvc.Examples.Razor.Models.Ninja>"),
                    new TextNode("\r\n\r\n"),
                    new ForeachNode("@foreach", "(var ninja in Model)",
                        [
                            new TextNode("- "),
                            new VariableNode("@ninja.Name"),
                            new TextNode("\r\n")
                        ])
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
                    new ModelTypeDeclarationNode("@model", "List<Cli.Mvc.Examples.Razor.Models.Ninja>"),
                    new TextNode("\r\n\r\n"),
                    new IfNode("@if", "(Model.Count == 0)",
                        [
                            new TextNode("Nothing to show\r\n")
                        ])
                ]);

            var parser = new Parser();

            var tree = parser.Parse(text);

            Assert.Equivalent(expectedTree, tree, true);
        }
    }
}
