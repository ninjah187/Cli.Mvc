using Cli.Mvc.Runtime;
using RazorLight;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Cli.Mvc.RazorLight
{
    public class RazorView<T> : IActionResult
    {
        public ICommandContext CommandContext { get; }
        public T Model { get; }

        public RazorView(ICommandContext commandContext, T model)
        {
            CommandContext = commandContext;
            Model = model;
        }

        public void Render()
        {
            Console.WriteLine("Rendering razor view");

            var assembly = Assembly.GetCallingAssembly();

            var projectPath = Path.Combine(AppContext.BaseDirectory, "Views");

            var engine = new RazorLightEngineBuilder()
                // .SetOperatingAssembly(assembly)
                // .SetOperatingAssembly(Assembly.GetExecutingAssembly())
                // .UseFileSystemProject("C:/dev/Cli.Mvc/Cli.Mvc.Examples.Razor/Views")
                .UseFileSystemProject(projectPath)
                .UseMemoryCachingProvider()
                .Build();

            //var template = "Hello, @Model.Name";

            //var model = new { Name = "Karol" };

            //var task = engine.CompileRenderAsync("Hello/World.cshtml", model);

            // var key = "Ninjas/List.cshtml";

            var path = BuildPathToView();

            var task = engine.CompileRenderAsync(path, Model);

            // var task = engine.CompileRenderAsync("Ninjas/List.cshtml", Model);

            // var task = engine.CompileRenderStringAsync("unique-key", template, model);

            task.Wait();

            var result = task.Result;

            Console.WriteLine(result);
        }

        //string BuildPathToView()
        //{
        //    // TODO: instead of using reflection, pass this info to CommandContext - ControllerType and ActionTypes

        //    var stackTrace = new StackTrace();

        //    var callerFrame = stackTrace.GetFrame(1);
        //    var callingMethod = callerFrame.GetMethod();

        //    var actionName = callingMethod.Name;
        //    var controllerName = callingMethod.DeclaringType.Name;

        //    return $"{controllerName.Replace("Controller", "")}/{actionName}.cshtml";
        //}

        string BuildPathToView()
        {
            var controller = CommandContext.Route.Controller.Name.Replace("Controller", "");
            var action = CommandContext.Route.Method.Name;

            return $"{controller}/{action}.cshtml";
        }
    }
}
