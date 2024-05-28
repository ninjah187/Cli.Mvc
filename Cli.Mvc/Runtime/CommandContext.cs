using Cli.Mvc.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Runtime
{
    public class CommandContext : ICommandContext
    {
        public string Command { get; }
        public string Path { get; }
        public Params Arguments { get; }
        public Params Options { get; }

        public CommandContext(string command, string path, Params arguments, Params options)
        {
            Command = command;
            Path = path;
            Arguments = arguments;
            Options = options;
        }
    }
}
