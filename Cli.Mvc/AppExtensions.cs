using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    public static class AppExtensions
    {
        public static void Repl(this App app)
        {
            while (true)
            {
                Console.Write("> ");
                
                var command = Console.ReadLine();

                if (command == "exit")
                {
                    return;
                }

                app.Run(command);

                Console.WriteLine();
            }
        }
    }
}
