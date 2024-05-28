using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public class Test
    {
        class TestController : Controller
        {
            public IActionResult Hello()
            {
                return Ok("Hello world!");
            }
        }

        [Fact]
        public void CanBuildSimpleApp()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            Assert.NotNull(app);
        }

        [Fact]
        public async Task CanRunCommand()
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run("test hello");
            });

            var expectedOutput = new[] { "Hello world!" };

            Assert.Equal(expectedOutput, output);
        }
    }
}
