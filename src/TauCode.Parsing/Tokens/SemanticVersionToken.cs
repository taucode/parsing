using TauCode.Data.Text;

namespace TauCode.Parsing.Tokens;

public class SemanticVersionToken : ValueTokenBase<SemanticVersion>
{
    public SemanticVersionToken(
        int position,
        int consumedLength,
        SemanticVersion value)
        : base(
            position,
            consumedLength,
            value,
            value.ToString())
    {
    }
}