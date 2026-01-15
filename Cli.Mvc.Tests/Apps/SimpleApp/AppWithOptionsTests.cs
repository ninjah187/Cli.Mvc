using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class AppWithOptionsTests
    {
        class TestController : Controller
        {
            public IActionResult Hello([Option] string name)
            {
                name ??= "stranger";
                return Ok($"Hello, {name}!");
            }
        }

        [Theory]
        [InlineData("test hello --name Bob",                        "Hello, Bob!")]
        [InlineData("test hello --name Bob Charles Bobbington",     "Hello, Bob Charles Bobbington!")]
        [InlineData("test hello --name \"Bob\"",                    "Hello, Bob!")]
        [InlineData("test hello --name \"Bob Charles Bobbington\"", "Hello, Bob Charles Bobbington!")]
        [InlineData("test hello --name",                            "Hello, stranger!")]
        [InlineData("test hello",                                   "Hello, stranger!")]
        [InlineData("test hello --name \"\"",                       "Hello, !")]
        public async Task CanRunCommandWithOptionValueInQuotemarks(string command, string expectedOutput)
        {
            await ControllerTest.Run<TestController>(command, expectedOutput);
        }
    }
}
