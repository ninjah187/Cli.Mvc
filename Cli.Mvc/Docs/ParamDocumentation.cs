using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Docs
{
    internal class ParamDocumentation
    {
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }

        public ParamDocumentation(string name, string description, string type)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }
}
