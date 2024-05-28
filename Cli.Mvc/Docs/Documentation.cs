using Cli.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cli.Mvc.Docs
{
    internal class Documentation
    {
        readonly IRouter _router;

        public Documentation(IRouter router)
        {
            _router = router;
        }

        //public string GetDescription(string path)
        //{
        //    foreach (var route in _router.Routes)
        //    {
        //        if (route.Path == path)
        //        {
        //            // action
        //        }
        //        if (route.ControllerPath == path)
        //        {
        //            // controller
        //        }
        //    }
        //}

        public ActionDocumentation GetActionDocumentation(Route route)
        {
            var description = route.Method.GetCustomAttribute<DescriptionAttribute>()?.Text;

            var parameters = route
                .Method
                .GetParameters()
                .Select(param =>
                {
                    var isOption = param.GetCustomAttribute<OptionAttribute>() != null;
                    return new
                    {
                        isOption,
                        documentation = GetParamDocumentation(param)
                    };
                });

            var arguments = new List<ParamDocumentation>();
            var options = new List<ParamDocumentation>();

            foreach (var param in parameters)
            {
                if (param.isOption)
                {
                    options.Add(param.documentation);
                }
                else
                {
                    arguments.Add(param.documentation);
                }
            }

            return new ActionDocumentation(route.Path, description, arguments, options);
        }

        public ParamDocumentation GetParamDocumentation(ParameterInfo parameter)
        {
            var description = parameter.GetCustomAttribute<DescriptionAttribute>()?.Text;
            return new ParamDocumentation(parameter.Name, description, GetParameterTypeName(parameter));
        }

        static string GetParameterTypeName(ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(bool))
            {
                return "bool";
            }
            if (parameter.ParameterType == typeof(int))
            {
                return "int";
            }
            return parameter.ParameterType.Name.ToLower();
        }
    }
}
