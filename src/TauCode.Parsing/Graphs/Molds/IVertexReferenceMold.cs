namespace TauCode.Parsing.Graphs.Molds
{
    internal interface IVertexReferenceMold : IPartMold
    {
        string ReferencedVertexPath { get; set; }
    }
}
