using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;

namespace Tau.Kappa.Parsing
{
    public interface IParsingNode : IVertex
    {
        bool AcceptsToken(ILexicalToken token, IParsingResult parsingResult);
        void Act(ILexicalToken token, IParsingResult parsingResult);
    }
}
