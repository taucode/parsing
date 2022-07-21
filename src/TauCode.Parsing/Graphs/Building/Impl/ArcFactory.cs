using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    public class ArcFactory : IArcFactory
    {
        public IArc Create(IArcMold arcMold)
        {
            return new Arc();
        }
    }
}
