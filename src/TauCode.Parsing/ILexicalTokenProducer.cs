namespace TauCode.Parsing;

public interface ILexicalTokenProducer
{
    ILexicalToken? Produce(LexingContext context);
}