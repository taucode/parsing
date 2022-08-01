namespace TauCode.Parsing.Graphs.Molding;

public interface ILinkableMold : IScriptElementMold
{
    string? GetFullPath();

    bool IsEntrance { get; set; }
    bool IsExit { get; set; }

    IArcMold AddLinkTo(ILinkableMold head);
    IArcMold AddLinkTo(string headPath);

    IArcMold AddLinkFrom(ILinkableMold tail);
    IArcMold AddLinkFrom(string tailPath);

    IReadOnlyList<IArcMold> OutgoingArcs { get; }
    IReadOnlyList<IArcMold> IncomingArcs { get; }
}