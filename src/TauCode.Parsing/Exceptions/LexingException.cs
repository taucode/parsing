namespace TauCode.Parsing.Exceptions;

[Serializable]
public class LexingException : Exception
{
    public LexingException()
    {
    }

    public LexingException(string message)
        : base(message)
    {
    }

    public LexingException(string message, int? index)
        : base(message)
    {
        this.Index = index;
    }

    public LexingException(string message, Exception inner) : base(message, inner)
    {
    }

    public int? Index { get; }
}