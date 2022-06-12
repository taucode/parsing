using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo: internal?
    // todo clean, regions
    public class VertexMold : LinkableMoldBase, IVertexMold
    {
        #region Fields

        private string _typeAlias;

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

        //public override IVertexMold Entrance
        //{
        //    get => this;
        //    set => throw new InvalidOperationException();
        //}

        //public override IVertexMold Exit
        //{
        //    get => this;
        //    set => throw new InvalidOperationException();
        //}

        #endregion

        #region IVertexMold Members

        //public virtual string Type { get; set; }


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

        //protected override void Validat-eAndFinalizeImpl()
        //{
        //    base.ValidateAndFinalizeImpl();

        //    _typeAlias = (string)this.GetKeywordValue(":TYPE");
        //}
    }
}
