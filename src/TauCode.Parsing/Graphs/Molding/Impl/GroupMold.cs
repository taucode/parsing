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

        private bool _entranceVertexIsCached;
        private IVertexMold _cachedEntranceVertex;

        private bool _exitVertexIsCached;
        private IVertexMold _cachedExitVertex;

        private bool _fullPathIsCached;
        private string _cachedFullPath;

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

            _allElements.Add(scriptElement);

            if (scriptElement is ILinkableMold linkable)
            {
                _linkables.Add(linkable);
            }
        }

        #endregion

        #region Overridden

        public override string GetFullPath()
        {
            if (_fullPathIsCached)
            {
                return _cachedFullPath;
            }

            string fullPath;

            if (this.Owner == null)
            {
                if (this.Name == null)
                {
                    fullPath = null;
                }
                else
                {
                    fullPath = $"/{this.Name}/";
                }
            }
            else
            {
                var ownerFullPath = this.Owner.GetFullPath();
                if (ownerFullPath == null)
                {
                    fullPath = null;
                }
                else
                {
                    fullPath = $"{ownerFullPath}{this.Name}/";
                }
            }

            _fullPathIsCached = true;
            _cachedFullPath = fullPath;

            return _cachedFullPath;
        }

        #endregion
    }
}
