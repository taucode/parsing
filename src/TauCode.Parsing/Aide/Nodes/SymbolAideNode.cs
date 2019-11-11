﻿using System;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide.Nodes
{
    public class SymbolAideNode : ProcessingNode
    {
        private SymbolAideNode(Action<IToken, IContext> processor)
            : base(processor, null)
        {
        }

        public SymbolAideNode(Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            return token is SymbolAideToken;
        }
    }
}