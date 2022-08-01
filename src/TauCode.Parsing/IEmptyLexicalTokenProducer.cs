namespace TauCode.Parsing;

public interface IEmptyLexicalTokenProducer : ILexicalTokenProducer
{
    int Skip(LexingContext context);
}