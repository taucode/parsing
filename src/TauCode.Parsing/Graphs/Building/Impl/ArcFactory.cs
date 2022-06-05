using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    public class ArcFactory : IArcFactory
    {
        public bool Accepts(IArcMold arcMold)
        {
            throw new System.NotImplementedException();
        }

        public IArc Create(IArcMold arcMold)
        {
            return new Arc();
        }
    }
}
