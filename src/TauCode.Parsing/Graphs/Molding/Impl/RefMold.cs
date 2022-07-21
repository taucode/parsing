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
