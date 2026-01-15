using Cli.Mvc;

var app = new AppBuilder().Build();

void Run(string command)
{
    Console.WriteLine($"> {command}");
    app.Run(command);
    Console.WriteLine();
}

Run("--help");

Run("hello world --help");

Run("hello world");
