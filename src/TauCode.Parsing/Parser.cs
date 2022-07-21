using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool AllowsMultipleExecutions { get; set; }

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

        public void Parse(IList<ILexicalToken> tokens, IParsingResult parsingResult)
        {
            // todo: skip empty tokens (and ut)

            var context = new ParsingContext(tokens, parsingResult);
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


                // todo log
                IParsingNode realWinner = null; // 'real' means it is not an EndNode
                gotEndNode = false;
                FallbackNode fallbackNode = null;

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

                    if (currentNode is FallbackNode someFallbackNode)
                    {
                        fallbackNode = someFallbackNode;
                        continue;
                    }

                    var accepts = currentNode.Accepts(context);
                    // todo: check position is not touched.

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
                            var currentToken = context.GetCurrentToken();

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
                    fallbackNode?.Act(context);

                    if (gotEndNode)
                    {
                        // ok, nobody accepted but end node => current clause is over, let's start from beginning
                        if (context.Position == context.Tokens.Count)
                        {
                            // will we ever get here?!
                            throw new NotImplementedException();
                        }

                        if (!this.AllowsMultipleExecutions)
                        {
                            throw new ParsingException("End of clause expected.", null, context.GetCurrentToken());
                        }

                        gotEndNode = false;
                        currentNodes = this.GetInitialNodes(this.Root);
                        continue;
                    }

                    // unexpected token
                    throw new ParsingException(
                        "Unexpected token.",
                        currentNodes,
                        context.GetCurrentToken());
                }

                var versionBeforeAct = parsingResult.Version;
                var positionBeforeAct = context.Position;

                realWinner.Act(context);

                if (positionBeforeAct != context.Position)
                {
                    throw new NotImplementedException(); // node must not touch position
                }
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
