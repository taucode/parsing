using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public abstract class ScriptElementMoldBase : IScriptElementMold
    {
        protected ScriptElementMoldBase(IGroupMold owner)
        {
            this.Owner = owner;
        }

        public IGroupMold Owner { get; }
        public string Name { get; set; }
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }
}
