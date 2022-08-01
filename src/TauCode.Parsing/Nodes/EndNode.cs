using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes;

public sealed class EndNode : ParsingNodeBase
{
    public EndNode()
    {
    }

    protected override bool AcceptsImpl(ParsingContext parsingContext)
    {
        throw new ParsingException($"Idle node's '{nameof(Accepts)}' method should never be called.");
    }

    protected override void ActImpl(ParsingContext parsingContext)
    {
        throw new ParsingException($"Idle node's '{nameof(Act)}' method should never be called.");
    }

    protected override string? GetDataTag() => null;

    public override ILexicalTokenConverter? TokenConverter
    {
        get => throw new ParsingException($"End node's '{nameof(TokenConverter)}' property should never be get.");
        set => throw new ParsingException($"End node's '{nameof(TokenConverter)}' property should never be set.");
    }
}