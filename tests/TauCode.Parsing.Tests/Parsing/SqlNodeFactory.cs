﻿using System;
using System.Linq;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing
{
    public class SqlNodeFactory : NodeFactory
    {
        public SqlNodeFactory(string nodeFamilyName)
            : base(nodeFamilyName)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            var links = item.GetItemLinks();

            INode node;

            switch (car)
            {
                case "WORD":
                    node = new ExactWordNode(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value,
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-IDENT":
                    node = new IdentifierNode(
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-WORD":
                    node = new WordNode(
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SYMBOL":
                    node = new ExactPunctuationNode(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value.Single(),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-INT":
                    node = new IntegerNode(
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-STRING":
                    node = new StringNode(
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                default:
                    throw new NotSupportedException();
            }

            return node;
        }
    }
}
