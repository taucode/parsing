using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TauCode.Parsing.Exceptions
{
    public class ParsingException : ParsingExceptionBase
    {
        public ParsingException()
        {
        }

        public ParsingException(string message)
            : base(message)
        {
        }

        public ParsingException(string message, IReadOnlyCollection<IParsingNode> nodes, ILexicalToken token)
            : base(BuildMessageWithNodesAndToken(message, nodes, token))
        {
            this.Nodes = nodes;
            this.Token = token;
        }

        public ParsingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public IReadOnlyCollection<IParsingNode> Nodes { get; }

        public ILexicalToken Token { get; }

        private static string BuildMessageWithNodesAndToken(string message, IEnumerable<IParsingNode> nodes, ILexicalToken token)
        {
            var sb = new StringBuilder();
            sb.Append(message);

            if (nodes != null)
            {
                sb.AppendLine();
                sb.AppendLine("Node(s):");
                foreach (var node in nodes)
                {
                    if (node == null)
                    {
                        throw new ArgumentException($"'{nameof(nodes)}' cannot contain nulls.");
                    }

                    sb.AppendLine(node.GetTag());
                }
            }

            if (token != null)
            {
                sb.Append("Token: ");
                sb.Append($"[{token}]");
                sb.Append($" Position: {token.Position}");
            }

            return sb.ToString();
        }
    }
}
