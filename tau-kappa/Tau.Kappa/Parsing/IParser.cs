using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Tau.Kappa.Parsing
{
    public interface IParser
    {
        ILogger Logger { get; set; }
        IParsingNode Root { get; set; }
        void Parse(IReadOnlyList<ILexicalToken> tokens, IParsingResult result);
    }
}
