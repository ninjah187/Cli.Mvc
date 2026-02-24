namespace Cli.Mvc.ViewCompiler.Tests
{
    public class TokenizerTests
    {
        [Fact]
        public void CanTokenizeHelloWorld()
        {
            var template = "hello world";

            var expected = new List<Token>
            {
                new("hello", TokenType.Text, 0, 0), new(" ", TokenType.Whitespace, 0, 5), new("world", TokenType.Text, 0, 6)
            };

            var result = Tokenize(template);

            AssertResult(expected, result);
        }

        [Fact]
        public void CanTokenizeSimpleVariableAccess()
        {
            var template = "hello @Model.Name";

            var expected = new List<Token>
            {
                new("hello", TokenType.Text, 0, 0), new(" ", TokenType.Whitespace, 0, 5), new("@Model.Name", TokenType.Variable, 0, 6)
            };

            var result = Tokenize(template);

            AssertResult(expected, result);
        }

        [Fact]
        public void CanTokenizeModelDeclarationAndVariableAccess()
        {
            var template = """
            @model Cli.Mvc.Examples.Razor.Models.Ninja

            Hello @Model.Name
            """;

            var expected = new List<Token>
            {
                new("@model", TokenType.ModelTypeDeclaration, 0, 0), new(" ", TokenType.Whitespace, 0, 6), new("Cli.Mvc.Examples.Razor.Models.Ninja", TokenType.Text, 0, 7), new("\r\n", TokenType.Whitespace, 0, 42),
                new("\r\n", TokenType.Whitespace, 1, 0),
                new("Hello", TokenType.Text, 2, 0), new(" ", TokenType.Whitespace, 2, 5), new("@Model.Name", TokenType.Variable, 2, 6)
            };

            var result = Tokenize(template);

            AssertResult(expected, result);
        }

        [Fact]
        public void CanTokenizeForeach()
        {
            var template =
                """
                @model List<Cli.Mvc.Examples.Razor.Models.Ninja>

                @foreach (var ninja in Model)
                {
                    @ninja.Name
                }
                """;

            var expected = new List<Token>
            {
                new("@model", TokenType.ModelTypeDeclaration, 0, 0), new(" ", TokenType.Whitespace, 0, 6), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 0, 7), new("\r\n", TokenType.Whitespace, 0, 48),
                new("\r\n", TokenType.Whitespace, 1, 0),
                new("@foreach", TokenType.ForEach, 2, 0), new(" ", TokenType.Whitespace, 2, 8), new("(var", TokenType.Text, 2, 9), new(" ", TokenType.Whitespace, 2, 13), new("ninja", TokenType.Text, 2, 14), new(" ", TokenType.Whitespace, 2, 19), new("in", TokenType.Text, 2, 20), new(" ", TokenType.Whitespace, 2, 22), new("Model)", TokenType.Text, 2, 23), new("\r\n", TokenType.Whitespace, 2, 29),
                new("{", TokenType.LeftBrace, 3, 0), new("\r\n", TokenType.Whitespace, 3, 1),
                new(" ", TokenType.Whitespace, 4, 0), new(" ", TokenType.Whitespace, 4, 1), new(" ", TokenType.Whitespace, 4, 2), new(" ", TokenType.Whitespace, 4, 3), new("@ninja.Name", TokenType.Variable, 4, 4), new("\r\n", TokenType.Whitespace, 4, 15),
                new("}", TokenType.RightBrace, 5, 0),
            };

            var result = Tokenize(template);

            AssertResult(expected, result);
        }

        [Fact]
        public void CanTokenizeIf()
        {
            var template =
                """
                @model List<Cli.Mvc.Examples.Razor.Models.Ninja>

                @if (Model.Count == 0)
                {
                    Nothing to show
                }
                """;

            var expected = new List<Token>
            {
                new("@model", TokenType.ModelTypeDeclaration, 0, 0), new(" ", TokenType.Whitespace, 0, 6), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 0, 7), new("\r\n", TokenType.Whitespace, 0, 48),
                new("\r\n", TokenType.Whitespace, 1, 0),
                new("@if", TokenType.If, 2, 0), new(" ", TokenType.Whitespace, 2, 3), new("(Model.Count", TokenType.Text, 2, 4), new(" ", TokenType.Whitespace, 2, 16), new("==", TokenType.Text, 2, 17), new(" ", TokenType.Whitespace, 2, 19), new("0)", TokenType.Text, 2, 20), new("\r\n", TokenType.Whitespace, 2, 22),
                new("{", TokenType.LeftBrace, 3, 0), new("\r\n", TokenType.Whitespace, 3, 1),
                new(" ", TokenType.Whitespace, 4, 0), new(" ", TokenType.Whitespace, 4, 1), new(" ", TokenType.Whitespace, 4, 2), new(" ", TokenType.Whitespace, 4, 3), new("Nothing", TokenType.Text, 4, 4), new(" ", TokenType.Whitespace, 4, 11), new("to", TokenType.Text, 4, 12), new(" ", TokenType.Whitespace, 4, 14), new("show", TokenType.Text, 4, 15), new("\r\n", TokenType.Whitespace, 4, 19),
                new("}", TokenType.RightBrace, 5, 0),
            };

            var result = Tokenize(template);

            AssertResult(expected, result);
        }

        static IReadOnlyList<Token> Tokenize(string input)
        {
            return new Tokenizer().Tokenize(input);
        }

        static void AssertResult(IReadOnlyList<Token> expected, IReadOnlyList<Token> actual)
        {
            Assert.Equivalent(expected, actual, true);
        }
    }
}
