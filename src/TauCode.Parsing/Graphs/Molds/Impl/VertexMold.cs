using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    // todo clean, regions
    public class VertexMold : PartMoldBase, IVertexMold
    {
        #region Fields

        //private readonly Lazy<IVertexMold> _entrance;
        //private readonly Lazy<IVertexMold> _exit;

        private readonly List<IArcMold> _outgoingArcs;
        private readonly List<IArcMold> _incomingArcs;

        #endregion

        #region ctor

        public VertexMold(IGroupMold owner, Atom car)
            : base(owner, car)
        {
            if (this.Owner == null)
            {
                throw new NotImplementedException();
            }

            //_entrance = new Lazy<IVertexMold>(this);
            //_exit = new Lazy<IVertexMold>(this);

            _outgoingArcs = new List<IArcMold>();
            _incomingArcs = new List<IArcMold>();
        }

        #endregion

        #region Overridden

        public override string GetFullPath()
        {
            var ownerFullPath = this.Owner.GetFullPath();
            if (ownerFullPath == null)
            {
                return null;
            }

            return $"{ownerFullPath}/{this.Name}";
        }

        public override IVertexMold Entrance
        {
            get => this;
            set => throw new InvalidOperationException();
        }

        public override IVertexMold Exit
        {
            get => this;
            set => throw new InvalidOperationException();
        }

        #endregion

        #region IVertexMold Members

        public virtual string Type { get; set; }

        public virtual IArcMold AddLinkTo(IVertexMold head)
        {
            // todo checks
            var arcMold = new ArcMold(this.Owner)
            {
                Tail = this,
                Head = head,
            };

            _outgoingArcs.Add(arcMold);
            return arcMold;
        }

        public virtual IArcMold AddLinkTo(string headPath)
        {
            // todo checks

            var arcMold = new ArcMold(this.Owner)
            {
                Tail = this,
                HeadPath = headPath
            };

            _outgoingArcs.Add(arcMold);

            return arcMold;
        }

        public virtual IArcMold AddLinkFrom(IVertexMold tail)
        {
            throw new System.NotImplementedException();
        }

        public virtual IArcMold AddLinkFrom(string tailPath)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<IArcMold> OutgoingArcs => _outgoingArcs;
        public IReadOnlyList<IArcMold> IncomingArcs => _incomingArcs;

        #endregion
    }
}
