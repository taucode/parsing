using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    public sealed class VertexRefResolver : VertexMold
    {
        public VertexRefResolver(IGroupMold owner)
            : base(owner)
        {
        }

        #region Overridden (ScriptElementMoldBase)

        public override IDictionary<string, object> Properties => throw new NotImplementedException("error: invalid operation");

        #endregion

        #region Overridden (PartMoldBase)

        public override string GetFullPath()
        {
            throw new NotImplementedException("error: invalid operation");
        }

        #endregion
    }
}
