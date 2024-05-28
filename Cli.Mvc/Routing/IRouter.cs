using System;
using System.Collections.Generic;
using System.Text;
using Cli.Mvc.Runtime;

namespace Cli.Mvc.Routing
{
    public interface IRouter
    {
        IReadOnlyList<Route> Routes { get; }
        RuntimeAction Resolve(string command);
    }
}
