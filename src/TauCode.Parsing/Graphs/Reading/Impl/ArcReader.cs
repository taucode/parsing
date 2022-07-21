using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class ArcReader : ScriptElementReaderBase
    {
        public ArcReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
        {
            var arcMold = new ArcMold(owner, (Atom)element.GetCar());
            return arcMold;
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle.
        }

        protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle.
        }
    }
}
