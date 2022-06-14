namespace TauCode.Parsing.Graphs.Molding
{
    public interface IGroupRefMold : ILinkableMold
    {
        string ReferencedGroupPath { get; set; }
    }
}
