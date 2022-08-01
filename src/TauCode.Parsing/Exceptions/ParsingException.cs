using System.Text;

namespace TauCode.Parsing.Exceptions;

[Serializable]
public class ParsingException : ParsingExceptionBase
{
    public ParsingException()
    {
    }

    public ParsingException(string message)
        : base(message)
    {
    }

    public ParsingException(
        string message,
        IReadOnlyCollection<IParsingNode>? nodes,
        ILexicalToken? token)
        : base(
            BuildMessageWithNodesAndToken(
                message,
                nodes,
                token))
    {
        this.Nodes = nodes;
        this.Token = token;
    }

    public ParsingException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public IReadOnlyCollection<IParsingNode>? Nodes { get; }

    public ILexicalToken? Token { get; }

    private static string BuildMessageWithNodesAndToken(
        string message,
        IReadOnlyCollection<IParsingNode>? nodes,
        ILexicalToken? token)
    {
        var sb = new StringBuilder();
        sb.Append(message);

        if (nodes != null)
        {
            var nodeList = nodes.ToList();

            sb.AppendLine();
            sb.AppendLine("Node(s):");

            for (var i = 0; i < nodes.Count(); i++)
            {
                var node = nodeList[i];
                if (node == null)
                {
                    throw new ArgumentException($"'{nameof(nodes)}' cannot contain nulls.");
                }

                sb.Append(node.GetTag());
                if (i < nodes.Count - 1)
                {
                    sb.AppendLine();
                }
            }
        }

        if (token != null)
        {
            sb.AppendLine();

            sb.Append("Token: ");
            sb.Append($"[{token}] [{token.GetType().FullName}]");
            sb.Append($" Position: {token.Position}");
        }

        return sb.ToString();
    }
}
