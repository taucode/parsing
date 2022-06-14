using System;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    public class ArcMold : ScriptElementMoldBase, IArcMold
    {
        #region Fields

        private IVertexMold _tail;
        private string _tailPath;
        private IVertexMold _head;
        private string _headPath;

        #endregion

        #region ctor

        public ArcMold(IGroupMold owner, Atom car)
            : base(owner, car)
        {
        }

        public ArcMold(IGroupMold owner)
            : base(owner, Symbol.Create("arc"))
        {
        }

        #endregion

        #region Overridden

        protected override void ValidateAndFinalizeImpl()
        {
            throw new NotImplementedException("todo: check heads and tails");
        }

        #endregion

        #region IArcMold Members

        public IVertexMold Tail
        {
            get => _tail;
            set
            {
                this.CheckNotFinalized();
                _tail = value;
            }
        }

        public string TailPath
        {
            get => _tailPath;
            set
            {
                this.CheckNotFinalized();
                _tailPath = value;
            }
        }

        public IVertexMold Head
        {
            get => _head;
            set
            {
                this.CheckNotFinalized();
                _head = value;
            }
        }

        public string HeadPath
        {
            get => _headPath;
            set
            {
                this.CheckNotFinalized();
                _headPath = value;
            }
        }

        #endregion
    }
}
