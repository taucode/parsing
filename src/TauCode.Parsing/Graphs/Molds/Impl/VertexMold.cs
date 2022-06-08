using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    public class VertexMold : PartMoldBase, IVertexMold
    {
        #region Fields

        private readonly List<IArcMold> _outgoingArcs;
        private readonly List<IArcMold> _incomingArcs;

        #endregion

        #region ctor

        public VertexMold(IGroupMold owner)
            : base(owner)
        {
            if (this.Owner == null)
            {
                throw new NotImplementedException();
            }

            _outgoingArcs = new List<IArcMold>();
            _incomingArcs = new List<IArcMold>();
        }

        #endregion

        #region Overridden

        public override IVertexMold Entrance
        {
            get => this;
            internal set => throw new NotImplementedException();
        }

        public override IVertexMold Exit
        {
            get => this;
            internal set => throw new NotImplementedException();
        }

        #endregion

        #region IVertexMold Members

        public string Type { get; set; }

        public string FullPath
        {
            get
            {
                var ownerFullPath = this.Owner.FullPath;
                if (ownerFullPath == null)
                {
                    return null;
                }

                return $"{ownerFullPath}/{this.Name}";
            }
        }

        public IArcMold AddLinkTo(IVertexMold head)
        {
            // todo checks
            var arcMold = new ArcMold
            {
                Tail = this,
                Head = head,
            };

            _outgoingArcs.Add(arcMold);
            return arcMold;
        }

        public IArcMold AddLinkTo(string headPath)
        {
            // todo checks

            var arcMold = new ArcMold
            {
                Tail = this,
                HeadPath = headPath
            };

            _outgoingArcs.Add(arcMold);

            return arcMold;
        }

        public IArcMold AddLinkFrom(IVertexMold tail)
        {
            throw new System.NotImplementedException();
        }

        public IArcMold AddLinkFrom(string tailPath)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<IArcMold> OutgoingArcs => _outgoingArcs;
        public IReadOnlyList<IArcMold> IncomingArcs => _incomingArcs;

        #endregion
    }
}
