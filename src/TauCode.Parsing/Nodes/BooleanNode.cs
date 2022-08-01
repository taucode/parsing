using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes;

public class BooleanNode : ActionNode
{
    public BooleanNode(
        Action<ActionNode, ParsingContext> action)
        : base(action)
    {
    }

    public BooleanNode()
    {
    }

    protected override bool AcceptsImpl(ParsingContext parsingContext)
    {
        var token = parsingContext.GetCurrentToken();

        return token is BooleanToken; // todo: mind token converter? e.g. convert from strings like '0xdefeca'
    }

    protected override string? GetDataTag() => null;
}