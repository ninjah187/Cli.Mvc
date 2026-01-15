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

        public static IEnumerable<object[]> TestData()
        {
            yield return ["hello world", new List<Token> { new("hello", TokenType.Text), new(" ", TokenType.Whitespace), new("world", TokenType.Text) }];
            yield return ["hello @Name", new List<Token> { new("hello", TokenType.Text), new(" ", TokenType.Whitespace), new("@Name", TokenType.Variable) }];
            yield return ComplexCase();
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
    }
}
