namespace TauCode.Parsing.Tokens
{
    public class FilePathToken : TextTokenBase
    {
        public FilePathToken(
            int position,
            int consumedLength,
            string filePath)
            : base(
                position,
                consumedLength,
                filePath)
        {
        }
    }
}
