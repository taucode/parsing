using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes;

public class PredicateActionNode : ActionNode
{
    public PredicateActionNode(
        Func<PredicateActionNode, ParsingContext, bool> predicate,
        Action<ActionNode, ParsingContext> action)
        : base(action)
    {
        this.Predicate = predicate;
    }

    public PredicateActionNode()
    {
    }

    public Func<PredicateActionNode, ParsingContext, bool>? Predicate { get; set; }

    protected override bool AcceptsImpl(ParsingContext parsingContext)
    {
        var token = parsingContext.GetCurrentToken();

        if (this.Predicate == null)
        {
            throw new ParsingException("Predicate is not set.", new List<IParsingNode> { this }, token);
        }

        return this.Predicate(this, parsingContext);
    }

    protected override string? GetDataTag() => null;
}