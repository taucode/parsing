using System.Diagnostics;
using System.Text;
using TauCode.Data.Graphs;

namespace TauCode.Parsing;

[DebuggerDisplay("{GetTag()}")]
public abstract class ParsingNodeBase : Vertex, IParsingNode
{
    #region Abstract

    protected abstract bool AcceptsImpl(ParsingContext parsingContext);
    protected abstract void ActImpl(ParsingContext parsingContext);

    /// <summary>
    /// Returns readable tag of node's data, if there is any. Mostly for debug purposes.
    /// </summary>
    /// <returns>Readable tag of node's data.</returns>
    protected abstract string? GetDataTag();

    #endregion

    #region IParsingNode Members

    public virtual ILexicalTokenConverter? TokenConverter { get; set; }

    public bool Accepts(ParsingContext parsingContext)
    {
        if (parsingContext == null)
        {
            throw new ArgumentNullException(nameof(parsingContext));
        }

        var result = this.AcceptsImpl(parsingContext);
        return result;
    }

    public void Act(ParsingContext parsingContext)
    {
        if (parsingContext == null)
        {
            throw new ArgumentNullException(nameof(parsingContext));
        }

        this.ActImpl(parsingContext);
    }

    /// <summary>
    /// Returns readable tag, mostly for debugging purposes.
    /// </summary>
    /// <returns>Readable tag, mostly for debugging purposes.</returns>
    public string? GetTag()
    {
        var sb = new StringBuilder();
        var name = this.Name ?? "<null_name>";

        var dataTag = this.GetDataTag() ?? "<null_data>";

        sb.Append($"[{name}] [{dataTag}] [{this.GetType().FullName}]");

        return sb.ToString();
    }

    #endregion
}