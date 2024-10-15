using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class AppWithOptionalStringIntArguments
    {
        class TestController : Controller
        {
            public IActionResult Hello(string name, int? age)
            {
                if (age != null)
                {
                    return Ok($"Hello {name}, you are {age} years old!");
                }
                return Ok($"Hello {name}, I don't know your age!");
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
                app.Run("test hello Bob 18");
            });

            var expectedOutput = new[] { "Hello Bob, you are 18 years old!" };

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

            var expectedOutput = new[] { "Hello Bob, I don't know your age!" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
