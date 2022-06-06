namespace TauCode.Parsing.Graphs.Molds
{
    public interface IArcMold
    {
        IVertexMold Tail { get; set; }
        string TailPath { get; set; }
        IVertexMold Head { get; set; }
        string HeadPath { get; set; }
    }
}
