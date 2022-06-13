using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface ILexicalToken
    {
        /// <summary>
        /// Position within the original text.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Length of original text consumed by the given token.
        /// </summary>
        int ConsumedLength { get; }

        IDictionary<string, string> Properties { get; set; }
    }
}
