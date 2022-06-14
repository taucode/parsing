namespace TauCode.Parsing.Graphs.Molding
{
    internal interface IVertexReferenceMold : ILinkableMold
    {
        string ReferencedVertexPath { get; set; }
    }
}
