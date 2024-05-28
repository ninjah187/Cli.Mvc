using Cli.Mvc.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Cli.Mvc.Routing
{
    public class RouterBuilder
    {
        readonly Type[] _types;

        public RouterBuilder(Type[] types)
        {
            _types = types;
        }

        public IRouter Build()
        {
            var routes = new List<Route>();

            var controllers = _types
                .Where(type => typeof(Controller).IsAssignableFrom(type)) // TODO: add controller attribute
                .ToArray();

            foreach (var controller in controllers)
            {
                var controllerPath = Route.GetPath(controller);

                //var actions = controller
                //    .GetMethods(BindingFlags.Public)
                //    // .Where(IsActionMethod)
                //    .Select(method => new
                //    {
                //        method,
                //        routeAttribute = method.GetCustomAttribute<RouteAttribute>()
                //    })
                //    .ToArray();

                //var hasExplicitRoutes = actions.Any(action => action.routeAttribute != null);

                //routes.Add(new Route(controller, method, controllerPath))

                // Add following feature: if has explicit [Route], then only methods with [Route] are actions.

                foreach (var method in controller.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!IsActionMethod(method))
                    {
                        continue;
                    }

                    var actionPath = Route.GetPath(method);

                    routes.Add(new Route(controller, method, controllerPath, actionPath));
                }
            }
            return new Router(routes);
        }

        bool IsActionMethod(MethodInfo methodInfo)
        {
            if (methodInfo.GetCustomAttribute<RouteAttribute>() != null)
            {
                return true;
            }
            if (typeof(IActionResult).IsAssignableFrom(methodInfo.ReturnType))
            {
                return true;
            }
            return false;
        }
    }
}
