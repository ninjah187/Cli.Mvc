namespace Cli.Mvc.Tests
{
    public class AppTests
    {
        [Fact]
        public void BuildsApp()
        {
            var app = new AppBuilder().Build();

            Assert.NotNull(app);
        }
    }
}
