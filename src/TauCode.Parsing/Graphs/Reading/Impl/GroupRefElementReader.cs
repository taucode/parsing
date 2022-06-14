using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class GroupRefElementReader : ScriptElementReaderBase
    {
        #region ctor

        public GroupRefElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        #endregion

        #region Overridden

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
        {
            IScriptElementMold scriptElementMold = new GroupRefMold(owner);
            return scriptElementMold;
        }

        protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle.
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle.
        }

        #endregion
    }
}
