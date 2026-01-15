using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    internal class ViewRenderingCodeCompiler
    {
        public string Compile(string template)
        {
            var tree = new Parser().Parse(template);
            var emitter = new ViewRenderingCodeEmitter();
            return emitter.EmitRenderingCode(tree);
        }
    }
}
