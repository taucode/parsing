﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.ParsingNodes
{
    public sealed class EndNode : ParsingNodeBase
    {
        public static EndNode Instance { get; } = new EndNode();

        private EndNode()
        {
            this.Name = this.GetType().FullName;
        }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }
    }
}
