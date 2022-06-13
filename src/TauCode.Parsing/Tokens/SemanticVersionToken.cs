using TauCode.Data;

namespace TauCode.Parsing.Tokens
{
    public class SemanticVersionToken : TextToken
    {
        public SemanticVersionToken(
            int position,
            int consumedLength,
            SemanticVersion semanticVersion)
            : base(
                position,
                consumedLength,
                semanticVersion.ToString())
        {
            this.SemanticVersion = semanticVersion;
        }

        public SemanticVersion SemanticVersion { get; }
    }
}
