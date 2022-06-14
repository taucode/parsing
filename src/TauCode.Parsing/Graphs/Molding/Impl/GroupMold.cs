using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    public class GroupMold : LinkableMoldBase, IGroupMold
    {
        #region Fields

        private readonly List<IScriptElementMold> _allElements;
        private readonly List<ILinkableMold> _linkables;

        private IVertexMold _entranceVertex;
        private IVertexMold _exitVertex;

        #endregion

        #region ctor

        public GroupMold(IGroupMold owner, Atom car)
            : base(owner, car)
        {
            _allElements = new List<IScriptElementMold>();
            _linkables = new List<ILinkableMold>();
        }

        #endregion

        #region IGroupMold Members

        public IReadOnlyList<IScriptElementMold> AllElements => _allElements;
        public IReadOnlyList<ILinkableMold> Linkables => _linkables;

        public void Add(IScriptElementMold scriptElement)
        {
            if (scriptElement == null)
            {
                throw new ArgumentNullException(nameof(scriptElement));
            }

            this.CheckNotFinalized();

            if (!scriptElement.IsFinalized)
            {
                throw new NotImplementedException("error: inner must be finalized.");
            }

            _allElements.Add(scriptElement);

            if (scriptElement is ILinkableMold linkable)
            {
                _linkables.Add(linkable);

                if (linkable.IsEntrance)
                {
                    if (_entranceVertex != null)
                    {
                        throw new NotImplementedException("error: more than one entrance");
                    }

                    _entranceVertex = linkable.GetEntranceVertex();
                }

                if (linkable.IsExit)
                {
                    if (_exitVertex != null)
                    {
                        throw new NotImplementedException("error: more than one exit");
                    }

                    _exitVertex = linkable.GetExitVertex();
                }
            }
        }

        #endregion

        #region Overridden

        public override string GetFullPath()
        {
            this.CheckFinalized();

            if (this.Owner == null)
            {
                if (this.Name == null)
                {
                    return null;
                }
                else
                {
                    return $"/{this.Name}/";
                }
            }

            var ownerFullPath = this.Owner.GetFullPath();
            if (ownerFullPath == null)
            {
                return null;
            }

            return $"{ownerFullPath}{this.Name}/";
        }

        protected override IVertexMold GetEntranceVertexImpl() => _entranceVertex;

        protected override IVertexMold GetExitVertexImpl() => _exitVertex;

        #endregion
    }
}
