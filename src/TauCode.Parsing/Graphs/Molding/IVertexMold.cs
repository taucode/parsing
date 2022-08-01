namespace TauCode.Parsing.Graphs.Molding;

public interface IVertexMold : ILinkableMold
{
    string? TypeAlias { get; set; }
}