﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class BlockBuilder
    {
        public BlockBuilder(Boss boss, BlockDefinitionResult source)
        {
            // todo checks
            this.Boss = boss;
            this.Source = source;
            this.GraphNode = this.Boss.Squad.RegisterBuilder(this);

            this.ReferencedBlockNames = this
                .Source
                .Content
                .GetAllTokenResultsFromContent()
                .Select(x => x.Token)
                .Where(x =>
                    x is EnumToken<SyntaxElement> syntaxEnumToken &&
                    syntaxEnumToken.Value == SyntaxElement.BlockReference)
                .Cast<EnumToken<SyntaxElement>>()
                .Select(x => x.Name)
                .ToList();
        }

        public Boss Boss { get; }
        public Node<BlockBuilder> GraphNode { get; }
        public BlockDefinitionResult Source { get; }
        public List<string> ReferencedBlockNames { get; }

        internal void Resolve()
        {
            foreach (var referencedBlockName in this.ReferencedBlockNames)
            {
                var referencedBlockBuilder = this.Boss.Squad.GetBlockBuilder(referencedBlockName);
                this.GraphNode.DrawEdgeTo(referencedBlockBuilder.GraphNode);
            }
        }

        public void Build()
        {
            var outcome = this.CreateContentOutcome(this.Source.Content);
            throw new NotImplementedException();
        }

        private ContentOutcome CreateContentOutcome(IContent content)
        {
            var outcome = new ContentOutcome();

            foreach (var aideResult in this.Source.Content)
            {
                NodeBuilder nodeBuilder;

                if (aideResult is TokenResult tokenResult)
                {
                    nodeBuilder = new NodeBuilder(tokenResult, this.Boss.NodeFamily);
                    nodeBuilder.Build();
                }
                else if (aideResult is OptionalResult optionalResult)
                {
                    throw new NotImplementedException();
                }
                else if (aideResult is AlternativesResult alternativesResult)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }

                outcome.AddNode(nodeBuilder);
            }

            outcome.InitLinks();

            return outcome;
        }
    }
}
