using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    public class VertexMold : PartMoldBase, IVertexMold
    {
        #region ctor

        public VertexMold(IGroupMold owner)
            : base(owner)
        {
            if (this.Owner == null)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Overridden

        public override IVertexMold Entrance => this;
        public override IVertexMold Exit => this;

        #endregion

        #region IVertexMold Members

        public string Name { get; set; }
        public string Type { get; set; }
        public string FullPath => throw new NotImplementedException();
        public IArcMold AddLinkTo(IVertexMold head)
        {
            throw new System.NotImplementedException();
        }

        public IArcMold AddLinkTo(string headFullPath)
        {
            throw new System.NotImplementedException();
        }

        public IArcMold AddLinkFrom(IVertexMold tail)
        {
            throw new System.NotImplementedException();
        }

        public IArcMold AddLinkFrom(string tailFullPath)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<IArcMold> OutgoingArcs => throw new NotImplementedException();
        public IReadOnlyList<IArcMold> IncomingArcs => throw new NotImplementedException();

        #endregion
    }
}
