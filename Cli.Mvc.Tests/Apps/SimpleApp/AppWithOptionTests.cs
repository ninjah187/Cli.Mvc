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
        public async Task CanRunCommandWithOption()
        {
            var output = await TestHelpers.RunAppWithCommand<TestController>("test hello --polite");

            var expectedOutput = new[] { "Welcome dear World" };

            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public async Task CanRunCommandWithEmptyOption()
        {
            var output = await TestHelpers.RunAppWithCommand<TestController>("test hello");

            var expectedOutput = new[] { "Hi world" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
