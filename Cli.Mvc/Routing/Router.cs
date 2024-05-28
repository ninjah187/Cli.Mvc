using Cli.Mvc.Parsing;
using Cli.Mvc.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Routing
{
    public class Router : IRouter
    {
        // TODO: make Routes lazy for better performance (reduce using reflection)
        // in most scenarios, only single action is used, so no need for compilation
        public IReadOnlyList<Route> Routes { get; }

        public Router(IReadOnlyList<Route> routes)
        {
            Routes = routes;
        }

        public RuntimeAction Resolve(string command)
        {
            foreach (var route in Routes)
            {
                if (command.StartsWith(route.Path))
                {
                    var parameters = command.Replace(route.Path, "");

                    return new RuntimeAction(command, route, Parser.Parse(parameters));
                }
            }

            return new RuntimeAction(command, null, Parser.Parse(command));

            // TODO: review this code
            throw new NotImplementedException("I guess we should show message 'not found' + help here.");
        }
    }
}
