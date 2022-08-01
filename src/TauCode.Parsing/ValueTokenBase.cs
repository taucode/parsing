namespace TauCode.Parsing;

public class ValueTokenBase<T> : TextTokenBase, IValueToken<T>
{
    public ValueTokenBase(
        int position,
        int consumedLength,
        T value,
        string text)
        : base(position, consumedLength, text)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.Value = value;
    }

    public T Value { get; }

    public override string? ToString() => this.Value!.ToString();
}