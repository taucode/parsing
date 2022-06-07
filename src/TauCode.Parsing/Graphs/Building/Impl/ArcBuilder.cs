using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    public class ArcBuilder : IArcBuilder
    {
        public bool Accepts(IArcMold arcMold)
        {
            throw new System.NotImplementedException();
        }

        public IArc Build(IArcMold arcMold)
        {
            return new Arc();
        }
    }
}
