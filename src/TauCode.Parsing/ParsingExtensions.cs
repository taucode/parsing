using TauCode.Data.Graphs;

namespace TauCode.Parsing;

public static class ParsingExtensions
{
    // todo: might be a part of TauCode.Data
    public static IArc AddLink(this IParsingNode nodeFrom, IParsingNode nodeTo)
    {
        var arc = new Arc();
        arc.Connect(nodeFrom, nodeTo);

        return arc;
    }
}