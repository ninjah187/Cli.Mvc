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
            var output = await TestHelpers.RunAppWithCommand<TestController>("test hello Bob");

            var expectedOutput = new[] { "Hello, Bob!" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithSingleArgumentInsideQuotemarks()
        {
            var output = await TestHelpers.RunAppWithCommand<TestController>("test hello \"Bob\"");

            var expectedOutput = new[] { "Hello, Bob!" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithSingleArgumentWhenNoArgumentNotProvided()
        {
            var output = await TestHelpers.RunAppWithCommand<TestController>("test hello");

            var expectedOutput = new[] { "Hello, stranger!" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
