using Cli.Mvc.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class AppWithMultipleArgumentsTests
    {
        class TestController : Controller
        {
            public IActionResult Hello(string name, string city)
            {
                return Ok($"Hello {name} from city of {city}!");
            }
        }

        [Fact]
        public async Task CanRunCommandWithMultipleArguments()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello Bob London");
            });

            var expectedOutput = new[] { "Hello Bob from city of London!" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithMultipleArgumentsInsideQuotemarks()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello \"Bob Charles Bobbington\" \"London\"");
            });

            var expectedOutput = new[] { "Hello Bob Charles Bobbington from city of London!" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
