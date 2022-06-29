using TauCode.Data.Text;

namespace TauCode.Parsing.Tokens
{
    public class EmojiToken : ValueTokenBase<Emoji>
    {
        public EmojiToken(
            int position,
            int consumedLength,
            Emoji emoji)
            : base(
                position,
                consumedLength,
                emoji,
                emoji.Value)
        {
        }
    }
}
