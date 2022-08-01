using System.Text;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes;

public class MultiWordNode : ActionNode
{
    public MultiWordNode(
        Action<ActionNode, ParsingContext>? action,
        IEnumerable<string> values,
        bool ignoreCase)
        : base(action)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        var valueList = values.ToList();

        if (valueList.Any(x => x == null))
        {
            throw new ArgumentException($"'{nameof(values)}' cannot contain nulls.", nameof(values));
        }

        if (ignoreCase)
        {
            valueList = valueList
                .Select(x => x.ToLowerInvariant())
                .ToList();
        }

        this.Values = new HashSet<string>(valueList);
        this.IgnoreCase = ignoreCase;
    }

    public bool IgnoreCase { get; }

    public MultiWordNode(
        IEnumerable<string> values,
        bool ignoreCase)
        : this(null, values, ignoreCase)
    {
    }

    public HashSet<string> Values { get; }

    protected override bool AcceptsImpl(ParsingContext parsingContext)
    {
        var token = parsingContext.GetCurrentToken();

        if (token is WordToken wordToken)
        {
            var value = wordToken.Text;
            if (this.IgnoreCase)
            {
                value = value.ToLowerInvariant();
            }

            return this.Values.Contains(value);
        }

        return false;
    }

    protected override string GetDataTag()
    {
        var sb = new StringBuilder();

        var values = this.Values.ToList();

        for (var i = 0; i < values.Count; i++)
        {
            var value = values[i];
            sb.Append($"'{value}'");
            if (i < values.Count - 1)
            {
                sb.Append(", ");
            }
        }

        return sb.ToString();
    }
}