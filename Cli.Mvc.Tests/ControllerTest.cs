using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cli.Mvc.Tests
{
    internal static class ControllerTest
    {
        public static async Task Run<T>(string command, string expectedOutput)
        {
            var app = new AppBuilder()
                .UseTypes(typeof(T))
                .Build();

            var output = await ConsoleOut.Collect(() =>
            {
                app.Run(command);
            });

            Assert.Equal(new[] { expectedOutput }, output);
        }
    }
}
