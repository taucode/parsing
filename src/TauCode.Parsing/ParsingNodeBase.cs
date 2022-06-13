using System;
using System.Diagnostics;
using System.Text;
using TauCode.Data.Graphs;

namespace TauCode.Parsing
{
    [DebuggerDisplay("{GetTag()}")]
    public abstract class ParsingNodeBase : Vertex, IParsingNode
    {
        #region Abstract

        protected abstract bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult);
        protected abstract void ActImpl(ILexicalToken token, IParsingResult parsingResult);

        /// <summary>
        /// Returns readable tag of node's data, if there is any. Mostly for debug purposes.
        /// </summary>
        /// <returns>Readable tag of node's data.</returns>
        protected abstract string GetDataTag();

        #endregion

        #region IParsingNode Members

        public bool AcceptsToken(ILexicalToken token, IParsingResult parsingResult)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (parsingResult == null)
            {
                throw new ArgumentNullException(nameof(parsingResult));
            }

            var result = this.AcceptsTokenImpl(token, parsingResult);
            return result;
        }

        public void Act(ILexicalToken token, IParsingResult parsingResult)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (parsingResult == null)
            {
                throw new ArgumentNullException(nameof(parsingResult));
            }

            this.ActImpl(token, parsingResult);
        }

        /// <summary>
        /// Returns readable tag, mostly for debugging purposes.
        /// </summary>
        /// <returns>Readable tag, mostly for debugging purposes.</returns>
        public string GetTag()
        {
            var sb = new StringBuilder();
            var name = this.Name ?? "<null_name>";

            var dataTag = this.GetDataTag() ?? "<null_data>";

            sb.Append($"[{name}] [{dataTag}] [{this.GetType().FullName}]");

            return sb.ToString();
        }

        #endregion
    }
}
