using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class AbstractSyntaxTree(IReadOnlyList<Node> nodes)
    {
        public IReadOnlyList<Node> Nodes => nodes;

        public IEnumerable<Node> Traverse()
        {
            var toVisit = new Queue<Node>(Nodes);

            while (toVisit.Count > 0)
            {
                var node = toVisit.Dequeue();

                yield return node;

                switch (node)
                {
                    case ForeachNode @foreach:
                        toVisit.Enqueue(@foreach.Body);
                        break;

                    case IfNode @if:
                        toVisit.Enqueue(@if.Body);
                        break;

                    // TODO: add IChildren interface or something
                    case BlockNode block:
                        foreach (var child in block.Body)
                        {
                            toVisit.Enqueue(child);
                        }
                        break;
                }
            }
        }
    }
}
