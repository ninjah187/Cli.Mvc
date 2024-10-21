using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests.Apps.SimpleApp
{
    public static class TestHelpers
    {
        public static async Task<string[]> RunAppWithCommand<TestController>(string command) where TestController : class
        {
            var app = new AppBuilder()
                .UseTypes(typeof(TestController))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run(command);
            });

            return output;
        }
    }
}
