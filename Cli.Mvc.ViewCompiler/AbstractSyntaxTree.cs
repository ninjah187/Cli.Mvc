using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class AbstractSyntaxTree(IReadOnlyList<Node> nodes)
    {
        public IReadOnlyList<Node> Nodes => nodes;
    }
}
