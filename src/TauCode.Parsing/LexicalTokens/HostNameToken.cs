using TauCode.Data;

namespace TauCode.Parsing.LexicalTokens
{
    public class HostNameToken : TextToken
    {
        public HostNameToken(
            int position,
            int consumedLength,
            HostName hostName)
            : base(
                position,
                consumedLength,
                hostName.ToString())
        {
            this.HostName = hostName;
        }

        public HostName HostName { get; }
    }
}
