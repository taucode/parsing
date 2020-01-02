﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;

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

        private IEnumerable<ITextClass> ParseTextClasses(PseudoList arguments)
        {
            var textClasses = new List<ITextClass>();

            foreach (var argument in arguments)
            {
                ITextClass textClass;
                var symbolElement = (Symbol)argument;

                switch (symbolElement.Name)
                {
                    case "WORD":
                        textClass = WordTextClass.Instance;
                        break;

                    case "IDENTIFIER":
                        textClass = IdentifierTextClass.Instance;
                        break;

                    case "STRING":
                        textClass = StringTextClass.Instance;
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
