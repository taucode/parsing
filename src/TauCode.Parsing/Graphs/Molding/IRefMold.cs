namespace TauCode.Parsing.Graphs.Molding
{
    public interface IRefMold : ILinkableMold
    {
        string ReferencedPath { get; set; }
    }
}
