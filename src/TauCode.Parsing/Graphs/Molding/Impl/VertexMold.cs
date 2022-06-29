using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    public class VertexMold : LinkableMoldBase, IVertexMold
    {
        #region Fields

        private bool _fullPathIsCached;
        private string _cachedFullPath;

        #endregion

        #region ctor

        public VertexMold(IGroupMold owner, Atom car)
            : base(owner, car)
        {
            if (this.Owner == null)
            {
                throw new NotImplementedException();
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

            var ownerFullPath = this.Owner.GetFullPath();
            if (ownerFullPath == null)
            {
                fullPath = null;
            }
            else
            {
                fullPath = $"{ownerFullPath}/{this.Name}";
            }

            _cachedFullPath = fullPath;
            _fullPathIsCached = true;

            return _cachedFullPath;
        }

        public override void ProcessKeywords()
        {
            base.ProcessKeywords();
            this.TypeAlias = this.GetKeywordValue<string>(":TYPE", null);

            var linksTo = this.GetKeywordValue<List<string>>(":LINKS-TO", null);
            if (linksTo != null)
            {
                foreach (var link in linksTo)
                {
                    this.AddLinkTo(link);
                }
            }

            var linksFrom = this.GetKeywordValue<List<string>>(":LINKS-FROM", null);
            if (linksFrom != null)
            {
                foreach (var link in linksFrom)
                {
                    this.AddLinkFrom(link);
                }
            }
        }

        #endregion

        #region IVertexMold Members

        public virtual string TypeAlias { get; set; }

        #endregion
    }
}
