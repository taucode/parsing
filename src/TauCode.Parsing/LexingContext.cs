namespace TauCode.Parsing;

public class LexingContext
{
    public LexingContext(ReadOnlyMemory<char> input)
    {
        this.Input = input;
    }

    public readonly ReadOnlyMemory<char> Input;
    public int Position;
}