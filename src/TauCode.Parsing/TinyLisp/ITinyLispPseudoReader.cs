using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.TinyLisp
{
    public interface ITinyLispPseudoReader
    {
        PseudoList Read(IList<ILexicalToken> tokens);
    }
}
