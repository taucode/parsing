namespace TauCode.Parsing
{
    public static class ParsingHelper
    {
        public static ILexicalToken GetCurrentToken(this ParsingContext parsingContext)
        {
            // todo checks
            return parsingContext.Tokens[parsingContext.Position];
        }
    }
}
