using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes;

public class Int32Node : ActionNode
{
    public Int32Node(
        Action<ActionNode, ParsingContext> action)
        : base(action)
    {
    }

    public Int32Node()
    {
    }

    protected override bool AcceptsImpl(ParsingContext parsingContext)
    {
        var token = parsingContext.GetCurrentToken();
        return token is Int32Token; // todo: mind token converter? e.g. convert from strings like '0xdefeca'
    }

    protected override string? GetDataTag() => null;
}