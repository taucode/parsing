using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class UriToken : TextToken
    {
        public UriToken(int position, int consumedLength, Uri uri)
            : base(position, consumedLength, uri.ToString())
        {
            this.Uri = uri;
        }

        public Uri Uri { get; }
    }
}
