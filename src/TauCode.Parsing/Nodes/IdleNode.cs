using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes
{
    public sealed class IdleNode : ParsingNodeBase
    {
        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new ParsingException($"Idle node's '{nameof(AcceptsToken)}' method should never be called.");
        }

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new ParsingException($"Idle node's '{nameof(Act)}' method should never be called.");
        }

        protected override string GetDataTag() => null;
    }
}
