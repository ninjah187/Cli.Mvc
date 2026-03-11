using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cli.Mvc.ViewCompiler
{
    public class Parser
    {
        public AbstractSyntaxTree Parse(string text)
        {
            var tokens = new Tokenizer().Tokenize(text);
            return Parse(tokens);
        }

        public AbstractSyntaxTree Parse(IReadOnlyList<Token> tokens)
        {
            var stack = new Stack<Token>(tokens.Reverse());

            var nodes = new List<Node>();

            while (stack.Count > 0)
            {
                var node = ProcessNode(stack);
                nodes.Add(node);
            }

            var tree = new AbstractSyntaxTree(nodes);

            return tree;
        }

        public static IReadOnlyList<Node> Parse_2(IReadOnlyList<Token> tokens)
        {
            if (tokens.Count == 0)
            {
                return [];
            }

            var stack = new Stack<Token>(tokens.Reverse());

            var nodes = new List<Node>();

            while (stack.Count > 0)
            {
                var node = ProcessNode(stack);
                nodes.Add(node);
            }

            return nodes;
        }

        static Node ProcessNode(Stack<Token> stack)
        {
            var token = stack.Pop();

            var node = Process(
                token,
                stack,
                Text,
                Variable,
                ModelTypeDeclaration,
                Foreach,
                If,
                Block
            );

            if (node == null)
            {
                throw new ParserException($"Could not parse token: {token.Value}");
            }

            return node;
        }

        static Node? Process(Token token, Stack<Token> stack, params Func<Token, Stack<Token>, Node?>[] parsers)
        {
            foreach (var parser in parsers)
            {
                var node = parser(token, stack);

                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        static Node? Text(Token token, Stack<Token> stack)
        {
            //var value = "";

            //while (token.Type == TokenType.Text || token.Type == TokenType.Whitespace)
            //{
            //    value += token.Value;

            //    if (stack.Count == 0)
            //    {
            //        break;
            //    }

            //    token = stack.Pop();
            //}

            if (token.Type == TokenType.Text || token.Type == TokenType.Whitespace)
            {
                // var value = token.Value;

                //var nextToken = stack.Pop();

                //while (nextToken.Type == TokenType.Text || nextToken.Type == TokenType.Whitespace)
                //{
                //    value += nextToken.Value;

                //    if (stack.Count == 0)
                //    {
                //        break;
                //    }

                //    nextToken = stack.Pop();
                //}

                //stack.Push(nextToken);

                var nextTokens = PopWhile(stack, t => t.Type == TokenType.Text ||  t.Type == TokenType.Whitespace);

                // value += string.Join("", nextTokens.Select(t => t.Value));

                return new TextNode(Merge(token, nextTokens));
            }

            return null;
        }

        static IReadOnlyList<Token> Merge(Token token, IEnumerable<Token> tokens)
        {
            var result = new List<Token> { token };
            result.AddRange(tokens);
            return result;
        }

        static IEnumerable<Token> PopWhile(Stack<Token> stack, Func<Token, bool> predicate)
        {
            while (stack.Count > 0)
            {
                var token = stack.Peek();
                // var token = stack.Pop();

                if (predicate(token))
                {
                    stack.Pop();
                    yield return token;
                }
                else
                {
                    // stack.Push(token);
                    break;
                }
            }
        }

        static Node? Variable(Token token, Stack<Token> stack)
        {
            if (token.Type == TokenType.Variable)
            {
                return new VariableNode(token);
            }

            return null;
        }

        static Node? ModelTypeDeclaration(Token token, Stack<Token> stack)
        {
            if (token.Type == TokenType.ModelTypeDeclaration)
            {
                var spacebar = stack.Pop();

                if (!(spacebar.Type == TokenType.Whitespace && spacebar.Value == " "))
                {
                    throw new ParserException($"Spacebar expected after @model keyword");
                }

                var modelType = stack.Pop();

                if (modelType.Type != TokenType.Text)
                {
                    throw new ParserException($"Model type identifier expected after @model keyword");
                }

                // Swallow trailing line break.
                stack.Pop();

                return new ModelTypeDeclarationNode(token, modelType);
            }

            return null;
        }

        static Node? Foreach(Token token, Stack<Token> stack)
        {
            if (token.Type != TokenType.ForEach)
            {
                return null;
            }

            stack.PopWhile(t => t.Value == " "); // skip spaces

            var headerTokens = stack
                .PopUntil(token => token.Value.EndsWith(")"))
                .Where(token => token.Type != TokenType.Whitespace)
                .ToList();
            var condition = string.Join("", headerTokens.Select(t => t.Value));

            stack.PopWhile(t => t.Value == "\r\n"); // skip line breaks

            //var bodyTokens = stack.PopUntil(t => t.Type == TokenType.RightBrace).ToList();
            ////bodyTokens = [.. bodyTokens.Take(bodyTokens.Count - 1).Skip(1)]; // skip '{' and '}'
            //bodyTokens = bodyTokens
            //    .TakeWhile(t => t.Type != TokenType.RightBrace)
            //    .SkipWhile(t => t.Type == TokenType.LeftBrace || t.Type == TokenType.Whitespace)
            //    .ToList();

            var bodyTokens = stack.PopUntil(t => t.Type == TokenType.RightBrace).ToList();

            var body = (BlockNode) Parse_2(bodyTokens)[0];

            return new ForeachNode(token, headerTokens, body);

            // return new ForeachNode(token.Value, condition, body);
            throw new NotImplementedException();
        }

        static Node? If(Token token, Stack<Token> stack)
        {
            if (token.Type != TokenType.If)
            {
                return null;
            }

            stack.PopWhile(t => t.Value == " "); // skip spaces

            var conditionTokens = stack
                .PopUntil(token => token.Value.EndsWith(")"))
                .Where(t => t.Type != TokenType.Whitespace)
                .ToList();

            // stack.PopWhile(t => t.Value == "\r\n"); // skip line breaks

            stack.PopWhile(t => t.Type == TokenType.Whitespace);

            //var bodyTokens = stack.PopUntil(t => t.Type == TokenType.RightBrace).ToList();
            //bodyTokens = bodyTokens
            //    .TakeWhile(t => t.Type != TokenType.RightBrace)
            //    .SkipWhile(t => t.Type == TokenType.LeftBrace || t.Type == TokenType.Whitespace)
            //    .ToList();

            var bodyTokens = stack.PopUntil(t => t.Type == TokenType.RightBrace).ToList();

            var body = bodyTokens.Count == 0 ? null : (BlockNode) Parse_2(bodyTokens)[0];

            return new IfNode(token, conditionTokens, body);
        }

        static Node? Block(Token token, Stack<Token> stack)
        {
            if (token.Type != TokenType.LeftBrace)
            {
                return null;
            }

            stack.PopWhile(IsLineBreak);

            var tokens = stack.PopUntil(t => t.Type == TokenType.RightBrace).ToList();

            var end = tokens.Count == 0 ? null : tokens[tokens.Count - 1];

            var bodyTokens = tokens.Count == 0 ? [] : tokens.GetRange(0, tokens.Count - 1);

            var body = Parse_2(bodyTokens);

            return new BlockNode(token, end, body);
        }

        static bool IsLineBreak(Token token)
        {
            return token.Type == TokenType.Whitespace && (token.Value == "\r\n" ||  token.Value == "\n");
        }
    }
}
