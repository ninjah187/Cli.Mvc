// See https://aka.ms/new-console-template for more information
using Cli.Mvc;
using Cli.Mvc.Example;

var app = new AppBuilder().Build();

// app.Run("hello world Karol 30");

app.Run("--help");
// app.Run("hello world --help");
// app.Run("hello people");

// app.Run(string.Join(" ", args));
