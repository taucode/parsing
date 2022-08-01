using Serilog;

namespace TauCode.Parsing;

public interface IParser
{
    bool AllowsMultipleExecutions { get; set; }
    ILogger? Logger { get; set; }
    IParsingNode? Root { get; set; }
    void Parse(IList<ILexicalToken> tokens, IParsingResult result);
}