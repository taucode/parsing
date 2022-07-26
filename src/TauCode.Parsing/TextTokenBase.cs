using System;

namespace TauCode.Parsing
{
    public abstract class TextTokenBase : LexicalTokenBase
    {
        protected TextTokenBase(
            int position,
            int consumedLength,
            string text)
            : base(position, consumedLength)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string Text { get; }

        public override string ToString() => this.Text;
    }
}
