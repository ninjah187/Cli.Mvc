using System;
using System.Net;
using System.Reflection.PortableExecutable;
using Cli.Mvc;
using Cli.Mvc.Example.controllers;

namespace Cli.Mvc.Example
{
    class Program
    {
        static void Main()
        {
            var app = new AppBuilder().Build();
            app.Run("hello world Michal 123");
        }
    }
}
