namespace TauCode.Parsing.LexicalTokens
{
    public class FilePathToken : TextToken
    {
        public FilePathToken(int position, int consumedLength, string path)
            : base(position, consumedLength, path)
        {
        }
    }
}
