using System;

namespace TauCode.Parsing
{
    public interface ILexicalTokenConverter
    {
        ILexicalToken Convert(ILexicalToken token, Type otherLexicalTokenType, IParsingResult parsingResult);

        TOtherLexicalTokenType Convert<TOtherLexicalTokenType>(ILexicalToken token, IParsingResult parsingResult)
            where TOtherLexicalTokenType : class, ILexicalToken;
    }
}
