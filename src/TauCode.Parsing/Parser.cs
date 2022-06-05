using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using Serilog;
using TauCode.Parsing.Graphs.Reading.Impl;
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
            //var currentNodes = new HashSet<IParsingNode>(new[] { this.Root });

            //var currentNodes = GetRoutes(this.Root);
            var currentNodes = this.GetInitialNodes(this.Root);
            ILexicalToken todoPrevToken = null;

            var gotEndNode = false;

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

                this.Logger?.Verbose($"Position: {context.Position} Current Token: {currentToken}");

                IParsingNode realWinner = null; // 'real' means it is not an EndNode
                gotEndNode = false;

                foreach (var currentNode in currentNodes)
                {
                    if (currentNode is IdleNode)
                    {
                        throw new NotImplementedException("error. should never happen");
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
                            throw new NotImplementedException();
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
                        //throw new NotImplementedException();
                        //currentNodes = new HashSet<IParsingNode>(new[] { this.Root }); // todo: use "cached" hashset
                        //currentNodes = GetRoutes(this.Root);
                        currentNodes = this.GetInitialNodes(this.Root);
                        todoPrevToken = null;
                        continue;
                    }

                    // unexpected token
                    throw new NotImplementedException("error: unexpected token");
                }

                var versionBeforeAct = parsingResult.Version;

                realWinner.Act(currentToken, parsingResult);

                var versionAfterAct = parsingResult.Version;

                if (versionAfterAct != versionBeforeAct + 1)
                {
                    //var log = TodoLogKeeper.Log.ToString();

                    throw new NotImplementedException("error: increase version.");
                }

                context.Position++;

                if (context.Position == 70)
                {
                    var log = TodoLogKeeper.Log.ToString();
                    var todo = 3;
                }

                currentNodes = GetRoutes(realWinner);
                if (currentNodes.Count == 0)
                {
                    throw new NotImplementedException(); // wtf
                }
            }
        }

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
    }
}
