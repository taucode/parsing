using System.Text;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes;

public abstract class ActionNode : ParsingNodeBase
{
    protected ActionNode(
        Action<ActionNode, ParsingContext>? action)
    {
        this.Action = action;
    }

    protected ActionNode()
    {
    }

    public Action<ActionNode, ParsingContext>? Action { get; set; }

    protected override void ActImpl(ParsingContext parsingContext)
    {
        if (this.Action == null)
        {
            var sb = new StringBuilder();
            sb.Append($"Node's '{nameof(Action)}' is not set. ");
            sb.Append($"Node type: '{this.GetType().FullName}'.");
            if (this.Name != null)
            {
                sb.Append($" Node name: '{this.Name}'.");
            }

            var message = sb.ToString();

            throw new ParsingException(message);
        }

        this.Action(this, parsingContext);
    }
}