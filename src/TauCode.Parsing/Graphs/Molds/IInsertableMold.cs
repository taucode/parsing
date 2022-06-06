namespace TauCode.Parsing.Graphs.Molds
{
    public interface IInsertableMold : IGraphPartMold
    {
        IVertexMold Entrance { get; set; }

        IVertexMold Exit { get; set; }
    }
}
