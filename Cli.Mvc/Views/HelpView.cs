using Cli.Mvc.Docs;
using Cli.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cli.Mvc.Views
{
    public class HelpView : View
    {
        // Maybe I should disallow injecting by field or property in view?
        // Instead of injecting IRouter, view's model should be Route[].
        [Service] readonly IRouter _router;
        [Service] readonly Documentation _documentation;

        readonly string _path; // make it injectable service?

        public HelpView(string path)
        {
            _path = path;
        }

        public override void Render()
        {
            if (_path == null)
            {
                ListCommands();
            }
            else
            {
                ShowCommandHelp();
            }
        }

        void ListCommands()
        {
            Line("Commands:");

            foreach (var route in _router.Routes)
            {
                var description = GetDescription(route.Method);

                Write($"- {route.Path}");
                Write(description == null ? null : $" - {description}");
                Line();
            }
        }

        void ShowCommandHelp()
        {
            var route = _router.Routes.Single(r => r.Path == _path);

            // var action = _documentation.GetActionDocumentation(route);

            var action = new Documentation(_router).GetActionDocumentation(route);


            if (action.Description != null)
            {
                Line();
                Line(action.Description);
                Line();
            }

            Line("Usage:");
            Write($"{action.Path}");

            if (action.Arguments.Any())
            {
                foreach (var arg in action.Arguments)
                {
                    Write($" <{arg.Name}:{arg.Type}>");
                }
            }

            if (action.Options.Any())
            {
                Write(" <options>");
            }

            Line();

            if (action.Arguments.Any())
            {
                Line();
                List("Arguments:", action.Arguments.Select(NameWithDescription));
            }

            if (action.Options.Any())
            {
                Line();
                List("Options:", action.Options.Select(NameWithDescription));
            }
        }

        static string NameWithDescription(ParamDocumentation param)
        {
            if (string.IsNullOrEmpty(param.Description))
            {
                return $"{param.Name}: {param.Type}";
            }
            return $"{param.Name}: {param.Type} - {param.Description}";
        }

        static string GetDescription(MethodInfo method)
        {
            return method.GetCustomAttribute<DescriptionAttribute>()?.Text;
        }
    }
}
