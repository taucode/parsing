using System;

namespace TauCode.Parsing.Graphs.Molds.Impl
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

        public ArcMold(IGroupMold owner)
            : base(owner)
        {
        }

        #endregion

        #region IArcMold Members

        public IVertexMold Tail
        {
            get => _tail;
            set
            {
                if (_tailPath != null)
                {
                    throw new NotImplementedException();
                }

                _tail = value;
            }
        }

        public string TailPath
        {
            get => _tailPath;
            set
            {
                if (_tail != null)
                {
                    throw new NotImplementedException();
                }

                _tailPath = value;
            }
        }

        public IVertexMold Head
        {
            get => _head;
            set
            {
                if (_headPath != null)
                {
                    throw new NotImplementedException();
                }

                _head = value;
            }
        }

        public string HeadPath
        {
            get => _headPath;
            set
            {
                if (_head != null)
                {
                    throw new NotImplementedException();
                }

                _headPath = value;
            }
        }

        #endregion
    }
}
