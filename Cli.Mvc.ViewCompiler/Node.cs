using System;
using System.Collections.Generic;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    //public class TextNode(string value)
    //{
    //    public string Value { get; } = value;
    //}

    public class Node(string value)
    {
        public string Value => value;
    }

    public class TextNode(string value) : Node(value)
    {
    }

    public class VariableNode(string value) : Node(value)
    {
    }

    public class ModelTypeDeclarationNode(string value, string modelType) : Node(value)
    {
        // TODO: consider whether this should be child TextNode or just string identifier declaration
        // public TextNode ModelType { get; } = modelType;

        public string ModelType { get; } = modelType;
    }

    public class ForeachNode(string value, string condition, IReadOnlyList<Node> body) : Node(value)
    {
        public string Condition { get; } = condition;
        public IReadOnlyList<Node> Body { get; } = body;
    }

    public class IfNode(string value, string condition, IReadOnlyList<Node> body) : Node(value)
    {
        public string Condition { get; } = condition;
        public IReadOnlyList<Node> Body { get; } = body;
    }
}
