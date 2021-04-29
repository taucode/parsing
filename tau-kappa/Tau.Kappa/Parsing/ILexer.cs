using System;
using System.Collections.Generic;

namespace Tau.Kappa.Parsing
{
    public interface ILexer
    {
        IEnumerable<ILexicalTokenProducer> Producers { get; set; }
        bool IgnoreEmptyTokens { get; set; }
        IReadOnlyList<ILexicalToken> Tokenize(ReadOnlyMemory<char> input);
        Action<LexingContext, ILexicalTokenProducer> OnBeforeTokenProduced { get; set; }
        Action<LexingContext, ILexicalToken> OnAfterTokenProduced { get; set; }
    }
}
