using Cli.Mvc.RazorLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc
{
    public static class ControllerExtensions
    {
        public static IActionResult View<T>(this Controller controller, T model = default)
        {
            return new RazorView<T>(controller.CommandContext, model);
        }
    }
}
