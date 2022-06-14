using TauCode.Data.Graphs;

namespace TauCode.Parsing
{
    public interface IParsingNode : IVertex
    {
        bool AcceptsToken(ILexicalToken token, IParsingResult parsingResult);
        void Act(ILexicalToken token, IParsingResult parsingResult);
        string GetTag();
    }
}
