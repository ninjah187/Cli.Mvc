using Cli.Mvc.Parsing;
using Cli.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Runtime
{
    public class CommandContext : ICommandContext
    {
        public string Command { get; }
        public Route Route { get; }
        public Params Arguments { get; }
        public Params Options { get; }

        public CommandContext(string command, Route route, Params arguments, Params options)
        {
            Command = command;
            Route = route;
            Arguments = arguments;
            Options = options;
        }
    }
}
