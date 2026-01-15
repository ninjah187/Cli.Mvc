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

        static Node ProcessNode(Stack<Token> stack)
        {
            var token = stack.Pop();

            var node = Process(
                token,
                stack,
                Text,
                Variable,
                ModelTypeDeclaration
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
                var value = token.Value;

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

                value += string.Join("", nextTokens.Select(t => t.Value));

                return new TextNode(value);
            }

            return null;
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
                return new VariableNode(token.Value);
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

                return new ModelTypeDeclarationNode(token.Value, modelType.Value);
            }

            return null;
        }
    }
}
