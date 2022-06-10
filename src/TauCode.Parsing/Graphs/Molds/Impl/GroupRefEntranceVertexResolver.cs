using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    public sealed class GroupRefEntranceVertexResolver : VertexMold
    {
        public GroupRefEntranceVertexResolver(IGroupRefMold keeper)
            : base(UnknownGroupMold.Instance, Nil.Instance)
        {
            this.Keeper = keeper ?? throw new ArgumentNullException(nameof(keeper));
        }

        public IGroupRefMold Keeper { get; }

        #region Overridden (ScriptElementMoldBase)

        public override IDictionary<string, object> KeywordValues => throw new NotImplementedException("error: invalid operation");

        #endregion

        #region Overridden (PartMoldBase)

        public override string GetFullPath()
        {
            throw new NotImplementedException("error: invalid operation");
        }

        #endregion

        #region Overridden (VertexMold)

        public override string Type
        {
            get => throw new NotImplementedException("error: invalid operation");
            set => throw new NotImplementedException("error: invalid operation");
        }

        public override IArcMold AddLinkTo(IVertexMold head)
        {
            throw new NotImplementedException("error: invalid operation");
        }

        public override IArcMold AddLinkTo(string headPath)
        {
            throw new NotImplementedException("error: invalid operation");
        }

        #endregion
    }
}
