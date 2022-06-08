using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using Serilog;
using TauCode.Parsing.ParsingNodes;

namespace TauCode.Parsing
{
    // todo regions, clean
    public class Parser : IParser
    {
        private readonly Dictionary<IParsingNode, HashSet<IParsingNode>> _routes;
        private IParsingNode _root;

        public Parser()
        {
            _routes = new Dictionary<IParsingNode, HashSet<IParsingNode>>();
        }

        public ILogger Logger { get; set; }

        public IParsingNode Root
        {
            get => _root;
            set
            {
                _routes.Clear();
                _root = value;
            }
        }

        public void Parse(IReadOnlyList<ILexicalToken> tokens, IParsingResult parsingResult)
        {
            // todo: skip empty tokens (and ut)
            // todo: parse multi-result script

            var context = new ParsingContext(tokens);
            var currentNodes = new HashSet<IParsingNode>(new[] { this.Root });

            //IParsingNode endNode = null;

            while (true)
            {
                if (context.Position == tokens.Count)
                {
                    if (currentNodes.Any(x => x is EndNode)) // todo: performance
                    {
                        // we can accept end
                        break;
                    }
                    else
                    {
                        throw new NotImplementedException(); // unexpected end
                    }
                }

                var currentToken = context.Tokens[context.Position];

                IParsingNode realWinner = null; // 'real' means it is not an EndNode
                //endNode = null;

                foreach (var currentNode in currentNodes)
                {
                    if (currentNode is EndNode)
                    {
                        // not our guy since we've got current token
                        continue;
                    }

                    var accepts = currentNode.AcceptsToken(currentToken, parsingResult);
                    if (accepts)
                    {
                        if (realWinner == null)
                        {
                            // we've got winner
                            realWinner = currentNode;
                        }
                        else
                        {
                            // we've got concurrency
                            throw new NotImplementedException();
                        }
                    }
                }

                if (realWinner == null)
                {
                    // unexpected token
                    throw new NotImplementedException();
                }

                realWinner.Act(currentToken, parsingResult);

                context.Position++;

                currentNodes = GetRoutes(realWinner);
                if (currentNodes.Count == 0)
                {
                    throw new NotImplementedException(); // wtf
                }
            }
        }

        private HashSet<IParsingNode> GetRoutes(IParsingNode node)
        {
            var contains = _routes.TryGetValue(node, out var list);
            if (contains)
            {
                return list;
            }

            var hashSet = new HashSet<IParsingNode>();
            AddNextNodesOf(node, hashSet);

            list = hashSet;

            _routes.Add(node, list);

            return list;
        }

        private static void AddNextNodesOf(IParsingNode node, HashSet<IParsingNode> hashSet)
        {
            var nextNodes = node
                .OutgoingArcs
                .Select(x => x.Head)
                .Cast<IParsingNode>()
                .ToList(); // todo temp only for debug; later remove this 'ToList()'

            foreach (var nextNode in nextNodes)
            {
                if (nextNode is IdleNode)
                {
                    AddNextNodesOf(nextNode, hashSet); // todo: if some 'hacker' creates IdleNode pointing to itself, we've get a stack overflow here
                }
                else
                {
                    hashSet.Add(nextNode);
                }
            }
        }
    }
}
