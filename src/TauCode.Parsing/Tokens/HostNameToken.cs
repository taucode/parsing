using TauCode.Data.Text;

namespace TauCode.Parsing.Tokens;

public class HostNameToken : ValueTokenBase<HostName>
{
    public HostNameToken(
        int position,
        int consumedLength,
        HostName hostName)
        : base(
            position,
            consumedLength,
            hostName,
            hostName.ToString())
    {
    }
}