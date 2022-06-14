using System;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    internal sealed class GroupRefExitVertexResolver : VertexMold
    {
        #region ctor

        internal GroupRefExitVertexResolver(IGroupRefMold keeper)
            : base(UnknownGroupMold.Instance, Nil.Instance)
        {
            this.Keeper = keeper ?? throw new ArgumentNullException(nameof(keeper));
        }

        #endregion

        #region Internal

        internal IGroupRefMold Keeper { get; }

        #endregion

        #region Overridden (LinkableMoldBase)

        public override string GetFullPath() => throw new NotImplementedException("error: invalid operation");

        public override string TypeAlias
        {
            get => throw new NotImplementedException("error: invalid operation");
            set => throw new NotImplementedException("error: invalid operation");
        }

        #endregion

        #region Overridden (VertexMold)

        public override IArcMold AddLinkFrom(IVertexMold head)
        {
            throw new NotImplementedException("error: invalid operation");
        }

        public override IArcMold AddLinkFrom(string headPath)
        {
            throw new NotImplementedException("error: invalid operation");
        }

        #endregion
    }
}
