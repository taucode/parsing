namespace TauCode.Parsing;

public interface ITextToken : ILexicalToken
{
    string Text { get; }
}