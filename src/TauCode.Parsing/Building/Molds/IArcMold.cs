namespace TauCode.Parsing.Building.Molds
{
    public interface IArcMold : IGraphPartMold
    {
        IVertexMold Tail { get; set; }
        string TailFullName { get; set; }
        IVertexMold Head { get; set; }
        string HeadFullName { get; set; }
    }
}
