using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    public class ParseDocsTask : Microsoft.Build.Utilities.Task
    {
        public string XmlPath { get; set; }

        public override bool Execute()
        {
            Console.WriteLine("Running ParseDocsTask");
            return true;
        }
    }
}
