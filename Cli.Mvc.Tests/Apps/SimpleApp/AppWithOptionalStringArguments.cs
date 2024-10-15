using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class AppWithOptionalStringArguments
    {
        class TestController : Controller
        {
            public IActionResult Hello(string name, string? city)
            {
                if (city != null)
                {
                    return Ok($"Hello {name} from city of {city}!");
                }
                return Ok($"Hello {name} from somewhere!");
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
                app.Run("test hello Bob London");
            });

            var expectedOutput = new[] { "Hello Bob from city of London!" };

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
                app.Run("test hello Bob");
            });

            var expectedOutput = new[] { "Hello Bob from somewhere!" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
