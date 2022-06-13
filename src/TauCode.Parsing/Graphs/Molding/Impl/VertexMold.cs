using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    public class VertexMold : LinkableMoldBase, IVertexMold
    {
        #region Fields

        private string _typeAlias;

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

            _outgoingArcs = new List<IArcMold>();
            _incomingArcs = new List<IArcMold>();
        }

        #endregion

        #region Overridden

        public override string GetFullPath()
        {
            if (!this.IsFinalized)
            {
                throw new NotImplementedException("error: finalize to get Full path.");
            }

            var ownerFullPath = this.Owner.GetFullPath();
            if (ownerFullPath == null)
            {
                return null;
            }

            return $"{ownerFullPath}/{this.Name}";
        }

        protected override IVertexMold GetEntranceVertexImpl() => this;

        protected override IVertexMold GetExitVertexImpl() => this;

        public override void ProcessKeywords()
        {
            base.ProcessKeywords();
            this.TypeAlias = (string)this.GetKeywordValue(":TYPE"); // todo: use GetKeywordValueAsString everywhere

            var linksTo = (List<string>)this.GetKeywordValue(":LINKS-TO");
            if (linksTo != null)
            {
                foreach (var link in linksTo)
                {
                    this.AddLinkTo(link);
                }
            }
        }

        #endregion

        #region IVertexMold Members

        public virtual string TypeAlias
        {
            get => _typeAlias;
            set
            {
                this.CheckNotFinalized();
                _typeAlias = value;
            }
        }

        public virtual IArcMold AddLinkTo(IVertexMold head)
        {
            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

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
            if (headPath == null)
            {
                throw new ArgumentNullException(nameof(headPath));
            }

            var arcMold = new ArcMold(this.Owner)
            {
                Tail = this,
                HeadPath = headPath,
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
