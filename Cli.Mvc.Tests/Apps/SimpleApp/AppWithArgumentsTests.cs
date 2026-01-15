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
            public IActionResult Hello(string? name)
            {
                name ??= "stranger";
                return Ok($"Hello, {name}!");
            }
        }

        [Theory]
        [InlineData("test hello Bob",                        "Hello, Bob!")]
        [InlineData("test hello Bob Charles Bobbington",     "Hello, Bob!")] // Only first argument is bound to action parameter. Rest is available in Controller.Arguments list.
        [InlineData("test hello \"Bob\"",                    "Hello, Bob!")]
        [InlineData("test hello \"Bob Charles Bobbington\"", "Hello, Bob Charles Bobbington!")]
        [InlineData("test hello",                            "Hello, stranger!")]
        [InlineData("test hello \"\"",                       "Hello, !")]
        public async Task CanRunCommand(string command, string expectedOutput)
        {
            await ControllerTest.Run<TestController>(command, expectedOutput);
        }
    }
}
