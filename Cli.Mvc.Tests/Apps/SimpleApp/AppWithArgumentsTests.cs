using Cli.Mvc.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class AppWithArgumentsTests
    {
        class TestController : Controller
        {
            public IActionResult Hello(string name)
            {
                name = name ?? "stranger";
                return Ok($"Hello, {name}!");
            }
        }

        [Fact]
        public async Task CanRunCommandWithSingleArgument()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello Bob");
            });

            var expectedOutput = new[] { "Hello, Bob!" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithSingleArgumentInsideQuotemarks()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello \"Bob\"");
            });

            var expectedOutput = new[] { "Hello, Bob!" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithSingleArgumentWhenNoArgumentNotProvided()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello");
            });

            var expectedOutput = new[] { "Hello, stranger!" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
