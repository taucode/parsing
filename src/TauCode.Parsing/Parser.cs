using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing
{
    public class Parser : IParser
    {
        #region Fields

        private readonly Dictionary<IParsingNode, HashSet<IParsingNode>> _routes;
        private IParsingNode _root;

        #endregion

        #region ctor

        public Parser()
        {
            _routes = new Dictionary<IParsingNode, HashSet<IParsingNode>>();
        }

        #endregion

        #region IParser Members

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

            var context = new ParsingContext(tokens);
            var currentNodes = this.GetInitialNodes(this.Root);

            var gotEndNode = false;

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
                        throw new ParsingException("Unexpected end.", currentNodes, null);
                    }
                }

                var currentToken = context.Tokens[context.Position];

                this.Logger?.Verbose($"Position: {context.Position} Current Token: {currentToken}");

                IParsingNode realWinner = null; // 'real' means it is not an EndNode
                gotEndNode = false;

                foreach (var currentNode in currentNodes)
                {
                    if (currentNode is IdleNode)
                    {
                        throw new ParsingException("Internal error: idle node questioned.");
                    }

                    if (currentNode is EndNode)
                    {
                        gotEndNode = true;
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
                            throw new ParsingException(
                                "Parsing node concurrency occurred.",
                                new List<IParsingNode>
                                {
                                    realWinner,
                                    currentNode,
                                },
                                currentToken);
                        }
                    }
                }

                if (realWinner == null)
                {
                    if (gotEndNode)
                    {
                        // ok, nobody accepted but end node => current clause is over, let's start from beginning
                        if (context.Position == context.Tokens.Count)
                        {
                            // will we ever get here?!
                            throw new NotImplementedException();
                        }

                        gotEndNode = false;
                        currentNodes = this.GetInitialNodes(this.Root);
                        continue;
                    }

                    // unexpected token
                    throw new ParsingException(
                        "Unexpected token.",
                        currentNodes,
                        currentToken);
                }

                var versionBeforeAct = parsingResult.Version;

                realWinner.Act(currentToken, parsingResult);

                var versionAfterAct = parsingResult.Version;

                if (versionAfterAct != versionBeforeAct + 1)
                {
                    throw new NotImplementedException("error: increase version.");
                }

                context.Position++;

                currentNodes = GetRoutes(realWinner);
                if (currentNodes.Count == 0)
                {
                    throw new NotImplementedException(); // wtf
                }
            }
        }

        #endregion

        #region Private

        private HashSet<IParsingNode> GetInitialNodes(IParsingNode root)
        {
            if (root is IdleNode)
            {
                return this.GetRoutes(root);
            }

            return new HashSet<IParsingNode>(new[] { root }); // todo: cache 'new[] { root }'
        }

        private HashSet<IParsingNode> GetRoutes(IParsingNode node)
        {
            var contains = _routes.TryGetValue(node, out var hashSet);
            if (contains)
            {
                return hashSet;
            }

            hashSet = new HashSet<IParsingNode>();
            AddNextNodesOf(node, hashSet);

            _routes.Add(node, hashSet);

            return hashSet;
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

        #endregion
    }
}
