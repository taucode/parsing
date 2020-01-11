﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Old;
using TauCode.Parsing.Old.Nodes;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlNodeFactory : NodeFactoryBase
    {
        public SqlNodeFactory(string nodeFamilyName)
            : base(nodeFamilyName)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "EXACT-TEXT":
                    node = new ExactTextNode(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value,
                        this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-TEXT":
                    node = new TextNode(
                        this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "PUNCTUATION":
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

                default:
                    throw new NotSupportedException();
            }

            return node;
        }

        private IEnumerable<IOldTextClass> ParseTextClasses(PseudoList arguments)
        {
            var textClasses = new List<IOldTextClass>();

            foreach (var argument in arguments)
            {
                IOldTextClass textClass;
                var symbolElement = (Symbol)argument;

                switch (symbolElement.Name)
                {
                    case "WORD":
                        textClass = OldWordTextClass.Instance;
                        break;

                    case "IDENTIFIER":
                        textClass = OldIdentifierTextClass.Instance;
                        break;

                    case "STRING":
                        textClass = OldStringTextClass.Instance;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                textClasses.Add(textClass);
            }

            return textClasses;
        }
    }
}
