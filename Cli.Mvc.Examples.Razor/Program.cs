// See https://aka.ms/new-console-template for more information
using Cli.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

static void WarmRoslyn()
{
    CSharpCompilation.Create("Warmup")
        .AddSyntaxTrees(CSharpSyntaxTree.ParseText("class A {}"))
        .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
        .Emit(Stream.Null);
}

// WarmRoslyn();

var app = new AppBuilder().Build();

// app.Run("hello world");

// app.Run("ninjas list");

app.Repl();
