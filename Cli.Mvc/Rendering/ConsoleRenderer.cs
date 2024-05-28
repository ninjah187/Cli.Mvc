using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Rendering
{
    public class ConsoleRenderer : IRenderer
    {
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
