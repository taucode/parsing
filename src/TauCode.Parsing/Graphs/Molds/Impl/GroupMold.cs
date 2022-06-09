using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public class GroupMold : PartMoldBase, IGroupMold
    {
        #region Fields

        private readonly List<IScriptElementMold> _scriptElements;

        private Lazy<IVertexMold> _entrance;
        private Lazy<IVertexMold> _exit;

        #endregion

        #region ctor

        public GroupMold(IGroupMold owner)
            : base(owner)
        {
            _scriptElements = new List<IScriptElementMold>();
        }

        #endregion

        #region IGroupMold Members

        public IReadOnlyList<IScriptElementMold> Content => _scriptElements;
        public void Add(IScriptElementMold scriptElement)
        {
            // todo checks
            _scriptElements.Add(scriptElement);
        }

        #endregion

        #region Overridden

        public override string GetFullPath()
        {
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

        public override IVertexMold Entrance { get; internal set; }

        public override IVertexMold Exit { get; internal set; }

        #endregion
    }
}
