using TauCode.Data.Graphs;

namespace TauCode.Parsing
{
    public interface IParsingNode : IVertex
    {
        ILexicalTokenConverter TokenConverter { get; set; }
        bool Accepts(ParsingContext parsingContext);
        void Act(ParsingContext parsingContext);
        string GetTag();
    }
}
