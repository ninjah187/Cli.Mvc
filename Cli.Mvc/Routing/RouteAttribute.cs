using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.Routing
{
    public class RouteAttribute : Attribute
    {
        public string Path { get; }

        public RouteAttribute(string path)
        {
            Path = path;
        }
    }
}
