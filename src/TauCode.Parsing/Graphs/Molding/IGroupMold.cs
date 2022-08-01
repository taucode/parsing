namespace TauCode.Parsing.Graphs.Molding;

public interface IGroupMold : ILinkableMold
{
    IReadOnlyList<IScriptElementMold> AllElements { get; }
    IReadOnlyList<ILinkableMold> Linkables { get; }
    void Add(IScriptElementMold scriptElementMold);
}