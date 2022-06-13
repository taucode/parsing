namespace TauCode.Parsing.Graphs.Molding
{
    public interface ILinkableMold : IScriptElementMold
    {
        string GetFullPath();

        bool IsEntrance { get; set; }
        bool IsExit { get; set; }

        IVertexMold GetEntranceVertex();
        IVertexMold GetExitVertex();
    }
}
