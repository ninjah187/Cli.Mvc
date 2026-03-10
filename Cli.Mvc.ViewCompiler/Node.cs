using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    //public class TextNode(string value)
    //{
    //    public string Value { get; } = value;
    //}

    public class Node
    {
        public IReadOnlyList<Token> Tokens { get; }

        public string Value { get; }

        //public int Line { get; }
        //public int Column { get; }

        protected Node(IReadOnlyList<Token> tokens)
        {
            Tokens = tokens;
            Value = BuildValue(tokens);
        }

        protected Node(Token token) : this([token])
        {
        }

        // TODO: keep this logic in Node or rather make Node dumb data structure and keep logic in Parser?
        protected virtual string BuildValue(IReadOnlyList<Token> tokens)
        {
            return string.Join("", tokens.Select(token => token.Value));
        }
    }

    public class TextNode(IReadOnlyList<Token> tokens) : Node(tokens)
    {
    }

    public class VariableNode(Token token) : Node(token)
    {
    }

    public class ModelTypeDeclarationNode(Token keyword, Token modelType) : Node([keyword, modelType])
    {
        // TODO: consider whether this should be child TextNode or just string identifier declaration
        // public TextNode ModelType { get; } = modelType;

        public Token Keyword { get; } = keyword;
        // public Token Spacebar { get; } = spacebar;
        public Token ModelType { get; } = modelType;
    }

    public class ForeachNode(Token keyword, IReadOnlyList<Token> header, BlockNode body) : Node([keyword, ..header])
    {
        // @foreach
        public Token Keyword { get; } = keyword;

        // public Token Spacebar { get; }

        // (var ninja in Ninjas)
        public IReadOnlyList<Token> Header { get; } = header;

        // public Token LeftBrace { get; }

        // public IReadOnlyList<Node> Body { get; } = body;

        // Later this could be another single node without block.
        public BlockNode Body { get; } = body;

        // public Token RightBrace { get; }

        protected override string BuildValue(IReadOnlyList<Token> tokens)
        {
            return string.Join(" ", tokens.Select(token => token.Value));
        }
    }

    public class BlockNode(Token start, Token end, IReadOnlyList<Node> body) : Node([start, end])
    {
        public Token Start { get; } = start;
        public Token End { get; } = end;
        public IReadOnlyList<Node> Body { get; } = body;
    }

    public class IfNode(Token keyword, IReadOnlyList<Token> header, BlockNode body) : Node([keyword, ..header])
    {
        public Token Keyword { get; } = keyword;
        public IReadOnlyList<Token> Header { get; } = header;
        public BlockNode Body { get; } = body;
    }

    //public class ForeachNode(string value, string condition, IReadOnlyList<Node> body) : Node(value)
    //{
    //    public string Condition { get; } = condition;
    //    public IReadOnlyList<Node> Body { get; } = body;
    //}

    //public class IfNode(string value, string condition, IReadOnlyList<Node> body) : Node(value)
    //{
    //    public string Condition { get; } = condition;
    //    public IReadOnlyList<Node> Body { get; } = body;
    //}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //public class Node(int line, int column, string value)
    //{
    //    public int Line { get; } = line;
    //    public int Column { get; } = column;

    //    public string Value => value;
    //}

    //public class TextNode(int line, int column, string value) : Node(line, column, value)
    //{
    //}

    //public class VariableNode(int line, int column, string value) : Node(line, column, value)
    //{
    //}

    //public class ModelTypeDeclarationNode(int line, int column, string value, string modelType) : Node(line, column, value)
    //{
    //    // TODO: consider whether this should be child TextNode or just string identifier declaration
    //    // public TextNode ModelType { get; } = modelType;

    //    public string ModelType { get; } = modelType;
    //}

    //public class ForeachNode(int line, int column, string value, string condition, IReadOnlyList<Node> body) : Node(line, column, value)
    //{
    //    public string Condition { get; } = condition;
    //    public IReadOnlyList<Node> Body { get; } = body;
    //}

    //public class IfNode(int line, int column, string value, string condition, IReadOnlyList<Node> body) : Node(line, column, value)
    //{
    //    public string Condition { get; } = condition;
    //    public IReadOnlyList<Node> Body { get; } = body;
    //}
}
