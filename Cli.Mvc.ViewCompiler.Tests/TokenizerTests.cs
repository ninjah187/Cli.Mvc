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
                new("hello", TokenType.Text, 0, 0, 0), new(" ", TokenType.Whitespace, 5, 0, 5), new("world", TokenType.Text, 6, 0, 6)
            };

            var result = Tokenize(template);

            AssertResult(expected, result);
        }

        [Fact]
        public void CanTokenizeTextWithVariable()
        {
            var template = "hello @Model.Name";

            var expected = new List<Token>
            {
                new("hello", TokenType.Text, 0, 0, 0), new(" ", TokenType.Whitespace, 5, 0, 5), new("@Model.Name", TokenType.Variable, 6, 0, 6)
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
                new("@model", TokenType.ModelTypeDeclaration, 0, 0, 0), new(" ", TokenType.Whitespace, 6, 0, 6), new("Cli.Mvc.Examples.Razor.Models.Ninja", TokenType.Text, 7, 0, 7), new("\r\n", TokenType.Whitespace, 42, 0, 42),
                new("\r\n", TokenType.Whitespace, 44, 1, 0),
                new("Hello", TokenType.Text, 46, 2, 0), new(" ", TokenType.Whitespace, 51, 2, 5), new("@Model.Name", TokenType.Variable, 52, 2, 6)
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
                    - @ninja.Name
                }
                """;

            var expected = new List<Token>
            {
                new("@model", TokenType.ModelTypeDeclaration, 0, 0, 0), new(" ", TokenType.Whitespace, 6, 0, 6), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 7, 0, 7), new("\r\n", TokenType.Whitespace, 48, 0, 48),
                new("\r\n", TokenType.Whitespace, 50, 1, 0),
                new("@foreach", TokenType.ForEach, 52, 2, 0), new(" ", TokenType.Whitespace, 60, 2, 8), new("(var", TokenType.Text, 61, 2, 9), new(" ", TokenType.Whitespace, 65, 2, 13), new("ninja", TokenType.Text, 66, 2, 14), new(" ", TokenType.Whitespace, 71, 2, 19), new("in", TokenType.Text, 72, 2, 20), new(" ", TokenType.Whitespace, 74, 2, 22), new("Model)", TokenType.Text, 75, 2, 23), new("\r\n", TokenType.Whitespace, 81, 2, 29),
                new("{", TokenType.LeftBrace, 83, 3, 0), new("\r\n", TokenType.Whitespace, 84, 3, 1),
                new(" ", TokenType.Whitespace, 86, 4, 0), new(" ", TokenType.Whitespace, 87, 4, 1), new(" ", TokenType.Whitespace, 88, 4, 2), new(" ", TokenType.Whitespace, 89, 4, 3), new("-", TokenType.Text, 90, 4, 4), new(" ", TokenType.Whitespace, 91, 4, 5), new("@ninja.Name", TokenType.Variable, 92, 4, 6), new("\r\n", TokenType.Whitespace, 103, 4, 17),
                new("}", TokenType.RightBrace, 105, 5, 0),
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
                new("@model", TokenType.ModelTypeDeclaration, 0, 0, 0), new(" ", TokenType.Whitespace, 6, 0, 6), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 7, 0, 7), new("\r\n", TokenType.Whitespace, 48, 0, 48),
                new("\r\n", TokenType.Whitespace, 50, 1, 0),
                new("@if", TokenType.If, 52, 2, 0), new(" ", TokenType.Whitespace, 55, 2, 3), new("(Model.Count", TokenType.Text, 56, 2, 4), new(" ", TokenType.Whitespace, 68, 2, 16), new("==", TokenType.Text, 69, 2, 17), new(" ", TokenType.Whitespace, 71, 2, 19), new("0)", TokenType.Text, 72, 2, 20), new("\r\n", TokenType.Whitespace, 74, 2, 22),
                new("{", TokenType.LeftBrace, 76, 3, 0), new("\r\n", TokenType.Whitespace, 77, 3, 1),
                new(" ", TokenType.Whitespace, 79, 4, 0), new(" ", TokenType.Whitespace, 80, 4, 1), new(" ", TokenType.Whitespace, 81, 4, 2), new(" ", TokenType.Whitespace, 82, 4, 3), new("Nothing", TokenType.Text, 83, 4, 4), new(" ", TokenType.Whitespace, 90, 4, 11), new("to", TokenType.Text, 91, 4, 12), new(" ", TokenType.Whitespace, 93, 4, 14), new("show", TokenType.Text, 94, 4, 15), new("\r\n", TokenType.Whitespace, 98, 4, 19),
                new("}", TokenType.RightBrace, 100, 5, 0),
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
