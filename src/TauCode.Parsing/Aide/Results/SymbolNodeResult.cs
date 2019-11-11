﻿using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Results
{
    public class SymbolNodeResult : NodeResult
    {
        public SymbolNodeResult(SymbolValue value, string tag)
            : base(tag)
        {
            this.Value = value;
        }

        public SymbolValue Value { get; }
    }
}