﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.ParsingNodes
{
    public class StringNode : ActionNode
    {
        public StringNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action)
            : base(action)
        {
        }

        public StringNode()
        {
        }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }
    }
}