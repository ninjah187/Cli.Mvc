using Cli.Mvc;

var app = new AppBuilder().Build();

void Run(string command)
{
    Console.WriteLine($"> {command}");
    app.Run(command);
    Console.WriteLine();
}

Run("--help");

Run("ninja --help");

Run("ninja add --help");

Run("ninja add Karol");

Run("ninja equip weapon --help");
