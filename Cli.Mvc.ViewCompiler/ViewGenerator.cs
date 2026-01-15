using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// TODO: rename to "ViewCompilation"
namespace Cli.Mvc.ViewCompiler
{
    [Generator]
    public class ViewGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // noop
        }

        public void Execute(GeneratorExecutionContext context)
        {
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            Console.WriteLine("Execute ViewGenerator");

            var @namespace = $"{context.Compilation.AssemblyName}.Views.Compiled";

            foreach (var file in context.AdditionalFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.Path);

                if (fileName != "World")
                {
                    continue;
                }

                var template = file.GetText(context.CancellationToken).ToString();

                var @class = GenerateViewClass(@namespace, fileName, template);

                context.AddSource($"{fileName}.g.cs", SourceText.From(@class, Encoding.UTF8));
            }
        }

        static string GenerateViewClass(string @namespace, string fileName, string template)
        {
            var emitter = new ViewRenderingCodeEmitter();

            var code = emitter.EmitClass(@namespace, fileName, template);

            return code;

            //var viewCompiler = new ViewRenderingCodeCompiler();

            //var renderingCode = viewCompiler.Compile(template);

            //var code = $$"""
            //using System;
            //using System.Text;

            //namespace {{@namespace}}
            //{
            //    public class {{fileName}}View
            //    {
            //        public string Render()
            //        {
            //            var sb = new StringBuilder();

            //            {{renderingCode}}

            //            return sb.ToString();
            //        }
            //    }
            //}
            //""";

            //return code;
        }
    }
}
