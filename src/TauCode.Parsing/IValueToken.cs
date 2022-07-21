namespace TauCode.Parsing
{
    public interface IValueToken<T> : ITextToken
    {
        T Value { get; }
    }
}
