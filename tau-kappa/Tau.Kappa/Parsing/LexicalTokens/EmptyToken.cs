using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public sealed class EmptyToken : IEmptyLexicalToken
    {
        public static readonly EmptyToken Instance = new EmptyToken();

        private EmptyToken()
        {   
        }

        public int Position => 0;
        public int ConsumedLength => 0;

        public IDictionary<string, string> Properties
        {
            get => null;
            set => throw new NotSupportedException();
        }
    }
}
