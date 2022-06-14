namespace TauCode.Parsing.Graphs.Molding
{
    public interface IArcMold : IScriptElementMold
    {
        IVertexMold Tail { get; set; }
        string TailPath { get; set; }
        IVertexMold Head { get; set; }
        string HeadPath { get; set; }
    }
}
