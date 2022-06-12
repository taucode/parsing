namespace TauCode.Parsing.Graphs.Molds
{
    internal interface IVertexReferenceMold : ILinkableMold
    {
        string ReferencedVertexPath { get; set; }
    }
}
