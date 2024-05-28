using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Cli.Mvc.Routing
{
    public class Route
    {
        public Type Controller { get; }
        public MethodInfo Method { get; }

        public string ControllerPath { get; }
        public string ActionPath { get; }

        public string Path => $"{ControllerPath} {ActionPath}";

        public Route(Type controller, MethodInfo method, string controllerPath, string actionPath)
        {
            Controller = controller;
            Method = method;
            ControllerPath = controllerPath;
            ActionPath = actionPath;
        }

        public static string GetPath(Type controller)
        {
            return GetPathFromAttribute(controller) ?? GetPathByConvention(controller.Name);
        }

        public static string GetPath(MethodInfo method)
        {
            return GetPathFromAttribute(method) ?? GetPathByConvention(method.Name);
        }

        static string GetPathFromAttribute(ICustomAttributeProvider provider)
        {
            var routeAttribute = (RouteAttribute)provider.GetCustomAttributes(typeof(RouteAttribute), true).FirstOrDefault();
            return routeAttribute?.Path;
        }

        static string GetPathByConvention(string name)
        {
            //var path = name
            //    .Replace("Controller", "")
            //    .SplitBy(char.IsUpper)
            //    .Select(x => x.ToLower())
            //    .ToArray();

            //return string.Join(" ", path);

            return name.Replace("Controller", "").ToLower();
        }
    }
}
