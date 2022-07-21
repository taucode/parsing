using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public abstract class LexicalTokenBase : ILexicalToken
    {
        #region Constructor

        protected LexicalTokenBase(
            int position,
            int consumedLength)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }
            this.Position = position;

            if (consumedLength <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.ConsumedLength = consumedLength;
        }

        #endregion

        #region ILexicalToken Members

        public int Position { get; }
        public int ConsumedLength { get; }
        public virtual IDictionary<string, string> Properties { get; set; }

        #endregion
    }
}
