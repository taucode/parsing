using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing
{
    public interface ILexicalTokenProducer
    {
        ILexicalToken Produce(LexingContext context);
    }
}
