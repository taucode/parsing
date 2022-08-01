namespace TauCode.Parsing.Exceptions;

[Serializable]
public class BuildingException : ParsingExceptionBase
{
    public BuildingException()
    {
    }

    public BuildingException(string message)
        : base(message)
    {
    }

    public BuildingException(string message, Exception inner)
        : base(message, inner)
    {
    }
}