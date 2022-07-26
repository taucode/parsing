using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    // todo regions
    public class RefMold : LinkableMoldBase, IRefMold
    {
        #region Fields

        private bool _fullPathIsCached;
        private string _cachedFullPath;

        private ILinkableMold _reference;

        #endregion

        public RefMold(IGroupMold owner, Atom car)
            : base(owner, car)
        {
        }

        public override void ProcessKeywords()
        {
            base.ProcessKeywords();

            this.ReferencedPath = this.GetKeywordValue<string>(":PATH");

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

        public override string GetFullPath()
        {
            // todo: copy-pasted from VertexMold.
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

        // todo not used
        private ILinkableMold GetReference()
        {
            if (_reference == null)
            {
                _reference = this.ResolvePath(this.ReferencedPath);
                if (_reference == null)
                {
                    throw new GraphException($"Could not resolve reference: '{this.ReferencedPath}'.");
                }
            }

            return _reference;
        }

        public string ReferencedPath { get; set; }
    }
}
