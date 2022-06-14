using System;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
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

        #region Overridden (LinkableMoldBase)

        public override string GetFullPath()
        {
            throw new NotImplementedException("error: invalid operation");
        }

        #endregion
    }
}
