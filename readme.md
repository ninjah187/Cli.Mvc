# Cli.Mvc

Cli.Mvc is a C# library designed to streamline the development of command-line interface (CLI) applications using an ASP.NET MVC-like pattern.

It leverages the familiar concept of controllers to handle command inputs, making it easier for developers to build, maintain, and scale CLI applications.

## Features

- **MVC Architecture**: Use controllers to handle commands, promoting clean separation of concerns.
- **Command Routing**: Automatically maps commands to corresponding controller actions.
- **Parameter Binding**: Supports binding command arguments and options to method parameters.
- **Dependency Injection**: Integrates with popular DI frameworks for easy service management.
- **Middleware Support**: Extend and customize command handling with middleware.

## Getting started

### Create a controller

```C#
using Cli.Mvc;

public class HelloController : Controller
{
    public IActionResult Greet(string name)
    {
        return Ok($"Hello, {name}!");
    }
}
```

### Build and run the app

```C#
using Cli.Mvc;

class Program
{
    static void Main(string[] args)
    {
        var app = new AppBuilder().Build();
        app.Run(args);  
    }
}
```

### Test your application

```
> hello greet Bob
Hello, Bob!
```
