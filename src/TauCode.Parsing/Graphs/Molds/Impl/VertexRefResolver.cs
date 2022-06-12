using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    public sealed class VertexRefResolver : VertexMold
    {
        public VertexRefResolver(IGroupMold owner)
            : base(owner, Symbol.Create("VERTEX-REF"))
        {
        }

        #region Overridden (ScriptElementMoldBase)

        

        #endregion

        #region Overridden (PartMoldBase)

        public override string GetFullPath()
        {
            throw new NotImplementedException("error: invalid operation");
        }

        #endregion
    }
}
