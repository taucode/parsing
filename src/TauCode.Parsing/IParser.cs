using System.Collections.Generic;
using Serilog;

namespace TauCode.Parsing
{
    public interface IParser
    {
        ILogger Logger { get; set; }
        IParsingNode Root { get; set; }
        void Parse(IReadOnlyList<ILexicalToken> tokens, IParsingResult result);
    }
}
