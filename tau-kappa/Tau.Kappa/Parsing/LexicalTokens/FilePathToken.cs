using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class FilePathToken : TextToken
    {
        public FilePathToken(int position, int consumedLength, string path)
            : base(position, consumedLength, path)
        {
        }
    }
}
