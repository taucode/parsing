using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl;

public class ArcMold : ScriptElementMoldBase, IArcMold
{
    #region ctor

    public ArcMold(IGroupMold owner, Atom car)
        : base(owner, car)
    {
    }

    public ArcMold(IGroupMold owner)
        : base(owner, Symbol.Create("arc"))
    {
    }

    #endregion

    #region Overridden

    public override void ProcessKeywords()
    {
        base.ProcessKeywords();

        this.TailPath = this.GetKeywordValue<string>(":TAIL-PATH");
        this.HeadPath = this.GetKeywordValue<string>(":HEAD-PATH");
    }

    #endregion

    #region IArcMold Members

    public ILinkableMold? Tail { get; set; }

    public string? TailPath { get; set; }

    public ILinkableMold? Head { get; set; }

    public string? HeadPath { get; set; }

    #endregion
}
