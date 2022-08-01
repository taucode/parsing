namespace TauCode.Parsing;

public abstract class LexicalTokenProducerBase : ILexicalTokenProducer
{
    public ILexicalToken? Produce(LexingContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Position >= context.Input.Length)
        {
            throw new InvalidOperationException("Position is out of range."); // todo ut
        }

        var token = this.ProduceImpl(context);
        return token;
    }

    protected abstract ILexicalToken? ProduceImpl(LexingContext context);
}