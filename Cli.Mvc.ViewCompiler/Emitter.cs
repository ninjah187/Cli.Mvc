using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class EmitterException : Exception
    {
        public EmitterException(string message) : base(message)
        {
        }
    }

    public interface IEmitter
    {
        string Emit(AbstractSyntaxTree tree);
    }

    public class ViewRenderingCodeEmitter 
    {
        //public string Emit(AbstractSyntaxTree tree)
        //{
        //    var sb = new StringBuilder();

        //    foreach (var node in tree.Nodes)
        //    {
        //        _ = node switch
        //        {
        //            TextNode text => sb.Append(text.Value),
        //            VariableNode variable => sb.Append(variable.Value),
        //            _             => throw new EmitterException("blah")
        //        };
        //    }

        //    return sb.ToString();
        //}

        public string EmitRenderingCode(AbstractSyntaxTree tree)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                var node = tree.Nodes[i];

                if (node is ModelTypeDeclarationNode)
                {
                    // Rendered in class.
                    continue;
                }

                sb.Append(EmitNode(node));

                if (i < tree.Nodes.Count - 1)
                {
                    sb.Append("\r\n");
                    sb.Append("            "); // 3 x 4 spaces = 4 tabs
                }
            }

            //foreach (var node in tree.Nodes)
            //{
            //    //_ = node switch
            //    //{
            //    //    TextNode text => sb.Append(EmitNode(text)),
            //    //    ModelTypeDeclarationNode modelTypeDeclarationNode => 
            //    //    // VariableNode variable => sb.Append(variable.Value),
            //    //    _ => throw new EmitterException($"Cannot emit node: {node.Value}")
            //    //};

            //    if (node is ModelTypeDeclarationNode)
            //    {
            //        // Rendered in class.
            //        continue;
            //    }

            //    sb.Append(EmitNode(node));

            //    sb.Append("\r\n");
            //    sb.Append("            "); // 3 x 4 spaces = 4 tabs
            //}

            return sb.ToString();
        }

        public string EmitClass(string @namespace, string fileName, string template)
        {
            var tree = new Parser().Parse(template);

            var modelDeclarationNode = tree.Nodes.OfType<ModelTypeDeclarationNode>().FirstOrDefault();
            var modelType = modelDeclarationNode?.ModelType;
            var modelNamespace = GetModelNamespace(modelType);

            var renderingCode = EmitRenderingCode(tree);

            var code = $$"""
            using System;
            using System.Text;

            namespace {{@namespace}}
            {
                public class {{fileName}}View
                {
                    {{ If(modelType, $"public {modelType} Model {{ get; }}") }}

                    public {{fileName}}View({{ If(modelType, $"{modelType} model") }})
                    {
                        {{ If(modelType, $"Model = model;") }}
                    }

                    public string Render()
                    {
                        var sb = new StringBuilder();

                        {{renderingCode}}

                        return sb.ToString();
                    }
                }
            }
            """;

            return code;
        }

        static string? GetModelNamespace(string? modelType)
        {
            if (modelType == null)
            {
                return null;
            }

            var split = modelType.Split('.');

            var @namespace = string.Join(".", split.Take(split.Length - 1));

            return @namespace;
        }

        static string? If(string? check, string insert)
        {
            if (check == null)
            {
                return "";
            }
            return insert;
        }

        string EmitNode(Node node)
        {
            return node switch
            {
                TextNode text => EmitNode(text),
                VariableNode variable => EmitNode(variable),
                _ => throw new EmitterException($"Cannot emit node: {node.Value}")
            };
        }

        string EmitNode(TextNode node)
        {
            var formattedValue = node
                .Value
                .Replace("\n", "\\n")
                .Replace("\r", "\\r");
            
            return $"""sb.Append("{formattedValue}");"""; // TODO: avoid string concatenations for performance
        }

        string EmitNode(VariableNode node)
        {
            return $"""sb.Append({node.Value.TrimStart('@')});""";
        }
    }
}
