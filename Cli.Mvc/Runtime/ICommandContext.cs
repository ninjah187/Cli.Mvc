using Cli.Mvc.Parsing;
using Cli.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Runtime
{
    public interface ICommandContext
    {
        string Command { get; }
        Route Route { get; }
        Params Arguments { get; }
        Params Options { get; }
    }
}
