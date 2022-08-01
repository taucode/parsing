using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl;

public abstract class LinkableMoldBase : ScriptElementMoldBase, ILinkableMold
{
    #region Fields

    private readonly List<IArcMold> _outgoingArcs;
    private readonly List<IArcMold> _incomingArcs;

    #endregion

    #region ctor

    protected LinkableMoldBase(IGroupMold? owner, Atom car)
        : base(owner, car)
    {
        _outgoingArcs = new List<IArcMold>();
        _incomingArcs = new List<IArcMold>();
    }

    #endregion

    #region Overridden

    public override void ProcessKeywords()
    {
        base.ProcessKeywords();

        this.IsEntrance = this.GetKeywordValue(":IS-ENTRANCE", false);
        this.IsExit = this.GetKeywordValue(":IS-EXIT", false);
    }

    #endregion

    #region ILinkableMold Members

    public abstract string? GetFullPath();

    public bool IsEntrance { get; set; }

    public bool IsExit { get; set; }

    public IArcMold AddLinkTo(ILinkableMold head)
    {
        if (head == null)
        {
            throw new ArgumentNullException(nameof(head));
        }

        var arcMold = new ArcMold(this.Owner!)
        {
            Tail = this,
            Head = head,
        };

        _outgoingArcs.Add(arcMold);
        return arcMold;
    }

    public IArcMold AddLinkTo(string headPath)
    {
        if (headPath == null)
        {
            throw new ArgumentNullException(nameof(headPath));
        }

        var arcMold = new ArcMold(this.Owner!)
        {
            Tail = this,
            HeadPath = headPath,
        };

        _outgoingArcs.Add(arcMold);

        return arcMold;
    }

    public IArcMold AddLinkFrom(ILinkableMold tail)
    {
        throw new System.NotImplementedException();
    }

    public IArcMold AddLinkFrom(string tailPath)
    {
        if (tailPath == null)
        {
            throw new ArgumentNullException(nameof(tailPath));
        }

        var arcMold = new ArcMold(this.Owner!)
        {
            TailPath = tailPath,
            Head = this,
        };

        _incomingArcs.Add(arcMold);

        return arcMold;
    }

    public IReadOnlyList<IArcMold> OutgoingArcs => _outgoingArcs;

    public IReadOnlyList<IArcMold> IncomingArcs => _incomingArcs;

    #endregion
}