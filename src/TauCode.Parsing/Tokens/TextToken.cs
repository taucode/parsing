using System;

namespace TauCode.Parsing.Tokens
{
    public abstract class TextToken : LexicalTokenBase
    {
        protected TextToken(int position, int consumedLength, string text)
            : base(position, consumedLength)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string Text { get; }

        public override string ToString() => this.Text;
    }
}
