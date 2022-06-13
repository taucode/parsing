using Serilog;

namespace TauCode.Parsing
{
    public interface IParsingResult
    {
        int Version { get; }
        void IncreaseVersion();
    }
}
