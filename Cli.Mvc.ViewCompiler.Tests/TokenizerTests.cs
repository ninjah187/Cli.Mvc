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
                new("hello", TokenType.Text, 0), new(" ", TokenType.Whitespace, 0), new("world", TokenType.Text, 0)
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
                new("hello", TokenType.Text, 0), new(" ", TokenType.Whitespace, 0), new("@Model.Name", TokenType.Variable, 0)
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
                new("@model", TokenType.ModelTypeDeclaration, 0), new(" ", TokenType.Whitespace, 0), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 0), new("\r\n", TokenType.Whitespace, 0),
                new("\r\n", TokenType.Whitespace, 1),
                new("@foreach", TokenType.ForEach, 2), new(" ", TokenType.Whitespace, 2), new("(var", TokenType.Text, 2), new(" ", TokenType.Whitespace, 2), new("ninja", TokenType.Text, 2), new(" ", TokenType.Whitespace, 2), new("in", TokenType.Text, 2), new(" ", TokenType.Whitespace, 2), new("Model)", TokenType.Text, 2), new("\r\n", TokenType.Whitespace, 2),
                new("{", TokenType.LeftBrace, 3), new("\r\n", TokenType.Whitespace, 3),
                new(" ", TokenType.Whitespace, 4), new(" ", TokenType.Whitespace, 4), new(" ", TokenType.Whitespace, 4), new(" ", TokenType.Whitespace, 4), new("@ninja.Name", TokenType.Variable, 4), new("\r\n", TokenType.Whitespace, 4),
                new("}", TokenType.RightBrace, 5),
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
                new("@model", TokenType.ModelTypeDeclaration, 0), new(" ", TokenType.Whitespace, 0), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text, 0), new("\r\n", TokenType.Whitespace, 0),
                new("\r\n", TokenType.Whitespace, 1),
                new("@if", TokenType.If, 2), new(" ", TokenType.Whitespace, 2), new("(Model.Count", TokenType.Text, 2), new(" ", TokenType.Whitespace, 2), new("==", TokenType.Text, 2), new(" ", TokenType.Whitespace, 2), new("0)", TokenType.Text, 2), new("\r\n", TokenType.Whitespace, 2),
                new("{", TokenType.LeftBrace, 3), new("\r\n", TokenType.Whitespace, 3),
                new(" ", TokenType.Whitespace, 4), new(" ", TokenType.Whitespace, 4), new(" ", TokenType.Whitespace, 4), new(" ", TokenType.Whitespace, 4), new("Nothing", TokenType.Text, 4), new(" ", TokenType.Whitespace, 4), new("to", TokenType.Text, 4), new(" ", TokenType.Whitespace, 4), new("show", TokenType.Text, 4), new("\r\n", TokenType.Whitespace, 4),
                new("}", TokenType.RightBrace, 5),
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
                new("@model", TokenType.ModelTypeDeclaration, 0), new(" ", TokenType.Whitespace, 0), new("Cli.Mvc.Examples.Razor.Models.Ninja", TokenType.Text, 0), new("\r\n", TokenType.Whitespace, 0),
                new("\r\n", TokenType.Whitespace, 1),
                new("Hello", TokenType.Text, 2), new(" ", TokenType.Whitespace, 2), new("@Model.Name", TokenType.Variable, 2)
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
