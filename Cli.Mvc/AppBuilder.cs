using Cli.Mvc.Rendering;
using Cli.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cli.Mvc
{
    public class AppBuilder
    {
        public IServiceCollection Services { get; } = new ServiceCollection();

        Type[] _types;
        Func<IRouter> _routerFactory;

        public AppBuilder UseConfiguration(string configuration)
        {
            return this;
        }

        public AppBuilder UseTypes(params Type[] types)
        {
            _types = types;
            return this;
        }

        public AppBuilder UseRouter<T>(Func<IRouter> factory = null) where T : IRouter
        {
            _routerFactory = factory ?? (() => (T) Activator.CreateInstance(typeof(T)));
            return this;
        }

        public AppBuilder AddSingleton<T>() where T : class
        {
            Services.AddSingleton<T>();
            return this;
        }

        public App Build()
        {
            var types = _types ?? Assembly.GetCallingAssembly().GetTypes();
            var router = _routerFactory?.Invoke() ?? new RouterBuilder(types).Build();

            Services.AddSingleton(typeof(IRouter), router);
            Services.AddSingleton(typeof(IRenderer), typeof(ConsoleRenderer));

            Startup(types);

            var serviceProvider = Services.BuildServiceProvider();

            return new App(serviceProvider);
        }

        void Startup(Type[] types)
        {
            var startupType = types.FirstOrDefault(type => type.Name == "Startup");

            if (startupType == null)
            {
                return;
            }

            var startup = Activator.CreateInstance(startupType);

            startupType.GetMethod("ConfigureServices")?.Invoke(startup, new[] { Services });
        }
    }
}
