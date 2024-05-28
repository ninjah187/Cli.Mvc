using Cli.Mvc.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Runtime
{
    public interface ICommandContext
    {
        string Command { get; }
        string Path { get; }
        Params Arguments { get; }
        Params Options { get; }
    }
}
