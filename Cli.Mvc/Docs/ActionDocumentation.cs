using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Docs
{
    internal class ActionDocumentation
    {
        public ActionDocumentation(string path, string description, IReadOnlyList<ParamDocumentation> arguments, IReadOnlyCollection<ParamDocumentation> options)
        {
            Path = path;
            Description = description;
            Arguments = arguments;
            Options = options;
        }

        public string Path { get; }
        public string Description { get; }
        public IReadOnlyList<ParamDocumentation> Arguments { get; }
        public IReadOnlyCollection<ParamDocumentation> Options { get; }
    }
}
