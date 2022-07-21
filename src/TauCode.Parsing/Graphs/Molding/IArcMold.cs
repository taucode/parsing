namespace TauCode.Parsing.Graphs.Molding
{
    public interface IArcMold : IScriptElementMold
    {
        ILinkableMold Tail { get; set; }
        string TailPath { get; set; }
        ILinkableMold Head { get; set; }
        string HeadPath { get; set; }
    }
}
