using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes;

public sealed class FallbackNode : ParsingNodeBase
{
    public Func<ParsingContext, string>? ExceptionMessageCreator { get; set; }

    private static string CreateDefaultMessage(ParsingContext parsingContext)
    {
        var token = parsingContext.GetCurrentToken();
        return $"Unexpected token: '{token}'.";
    }

    protected override bool AcceptsImpl(ParsingContext parsingContext) => true;

    protected override void ActImpl(ParsingContext parsingContext)
    {
        var messageCreator = this.ExceptionMessageCreator ?? CreateDefaultMessage;
        var message = messageCreator(parsingContext);
        throw new ParsingException(message);
    }

    protected override string? GetDataTag() => null;
}