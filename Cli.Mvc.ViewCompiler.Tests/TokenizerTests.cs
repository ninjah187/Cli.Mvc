namespace Cli.Mvc.ViewCompiler.Tests
{
    public class TokenizerTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Success(string input, IReadOnlyList<Token> expected)
        {
            var tokenizer = new Tokenizer();

            var result = tokenizer.Tokenize(input);

            Assert.Equivalent(expected, result, true);
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
                new("@model", TokenType.ModelTypeDeclaration), new(" ", TokenType.Whitespace), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text), new("\r\n", TokenType.Whitespace),
                new("\r\n", TokenType.Whitespace),
                new("@foreach", TokenType.ForEach), new(" ", TokenType.Whitespace), new("(var", TokenType.Text), new(" ", TokenType.Whitespace), new("ninja", TokenType.Text), new(" ", TokenType.Whitespace), new("in", TokenType.Text), new(" ", TokenType.Whitespace), new("Model)", TokenType.Text), new("\r\n", TokenType.Whitespace),
                new("{", TokenType.LeftBrace), new("\r\n", TokenType.Whitespace),
                new(" ", TokenType.Whitespace), new(" ", TokenType.Whitespace), new(" ", TokenType.Whitespace), new(" ", TokenType.Whitespace), new("@ninja.Name", TokenType.Variable), new("\r\n", TokenType.Whitespace),
                new("}", TokenType.RightBrace),
            };

            var tokenizer = new Tokenizer();

            var result = tokenizer.Tokenize(template);

            Assert.Equivalent(expected, result, true);
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
                new("@model", TokenType.ModelTypeDeclaration), new(" ", TokenType.Whitespace), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text), new("\r\n", TokenType.Whitespace),
                new("\r\n", TokenType.Whitespace),
                new("@if", TokenType.If), new(" ", TokenType.Whitespace), new("(Model.Count", TokenType.Text), new(" ", TokenType.Whitespace), new("==", TokenType.Text), new(" ", TokenType.Whitespace), new("0)", TokenType.Text), new("\r\n", TokenType.Whitespace),
                new("{", TokenType.LeftBrace), new("\r\n", TokenType.Whitespace),
                new(" ", TokenType.Whitespace), new(" ", TokenType.Whitespace), new(" ", TokenType.Whitespace), new(" ", TokenType.Whitespace), new("Nothing", TokenType.Text), new(" ", TokenType.Whitespace), new("to", TokenType.Text), new(" ", TokenType.Whitespace), new("show", TokenType.Text), new("\r\n", TokenType.Whitespace),
                new("}", TokenType.RightBrace),
            };

            var tokenizer = new Tokenizer();

            var result = tokenizer.Tokenize(template);

            Assert.Equivalent(expected, result, true);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return ["hello world", new List<Token> { new("hello", TokenType.Text), new(" ", TokenType.Whitespace), new("world", TokenType.Text) }];
            yield return ["hello @Name", new List<Token> { new("hello", TokenType.Text), new(" ", TokenType.Whitespace), new("@Name", TokenType.Variable) }];
            yield return ComplexCase();
            // yield return ForeachCase();
        }

        public static object[] ComplexCase() => [
            """
            @model Cli.Mvc.Examples.Razor.Models.Ninja

            Hello @Model.Name
            """,
            new List<Token>
            {
                new("@model", TokenType.ModelTypeDeclaration), new(" ", TokenType.Whitespace), new("Cli.Mvc.Examples.Razor.Models.Ninja", TokenType.Text), new("\r\n", TokenType.Whitespace),
                new("\r\n", TokenType.Whitespace),
                new("Hello", TokenType.Text), new(" ", TokenType.Whitespace), new("@Model.Name", TokenType.Variable)
            }
        ];

        //public static object[] ForeachCase() => [
        //    """
        //    @model List<Cli.Mvc.Examples.Razor.Models.Ninja>

        //    @foreach (var ninja in Model)
        //    {
        //        @ninja.Name
        //    }
        //    """,
        //    new List<Token>
        //    {
        //        new("@model", TokenType.ModelTypeDeclaration), new(" ", TokenType.Whitespace), new("List<Cli.Mvc.Examples.Razor.Models.Ninja>", TokenType.Text), new("\r\n", TokenType.Whitespace),
        //        new("\r\n", TokenType.Whitespace),
        //        new("@foreach", TokenType.ForEach), new(" ", TokenType.Whitespace), new("(var", TokenType.Text), new(" ", TokenType.Whitespace), new("ninja", TokenType.Text), new(" ", TokenType.Whitespace), new("in", TokenType.Text), new(" ", TokenType.Whitespace), new("Model)", TokenType.Text), new("\r\n", TokenType.Whitespace),
        //        new("{", TokenType.Text), new("\r\n", TokenType.Whitespace),
        //        new("    ", TokenType.Text), new("@ninja.Name", TokenType.Variable), new("\r\n", TokenType.Whitespace),
        //        new("}", TokenType.Text),
        //    }
        //];
    }
}
