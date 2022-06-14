using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes
{
    public class PredicateActionNode : ActionNode
    {
        public PredicateActionNode(
            Func<PredicateActionNode, ILexicalToken, IParsingResult, bool> predicate,
            Action<ActionNode, ILexicalToken, IParsingResult> action)
            : base(action)
        {
            this.Predicate = predicate;
        }

        public PredicateActionNode()
        {   
        }

        public Func<PredicateActionNode, ILexicalToken, IParsingResult, bool> Predicate { get; set; }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            if (this.Predicate == null)
            {
                throw new ParsingException("Predicate is not set.", new List<IParsingNode> { this }, token);
            }

            return this.Predicate(this, token, parsingResult);
        }

        protected override string GetDataTag() => null;
    }
}
