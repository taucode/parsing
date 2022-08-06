namespace TauCode.Parsing.Tokens;

public class UriToken : ValueTokenBase<Uri>
{
    public UriToken(
        int position,
        int consumedLength,
        Uri uri)
        : base(
            position,
            consumedLength,
            uri,
            uri.ToString())
    {
    }
}