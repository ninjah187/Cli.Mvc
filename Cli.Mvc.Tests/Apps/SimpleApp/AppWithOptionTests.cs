using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cli.Mvc;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class AppWithOptionTests
    {
        class TestController : Controller
        {
            public IActionResult Hello([Option("--polite", "To be polite or not.")] bool polite = true)
            {
                if (polite)
                {
                    return Ok($"Welcome dear World");
                }
                return Ok($"Hi world");
            }
        }

        [Fact]
        public async Task CanRunCommandWithOptionalArguments()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello --polite");
            });

            var expectedOutput = new[] { "Welcome dear World" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithEmptyOptionalArguments()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello");
            });

            var expectedOutput = new[] { "Hi world" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
