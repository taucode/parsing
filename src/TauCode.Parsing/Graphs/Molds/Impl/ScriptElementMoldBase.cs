using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public abstract class ScriptElementMoldBase : IScriptElementMold
    {
        protected ScriptElementMoldBase(IGroupMold owner, Atom car)
        {
            this.Owner = owner;
            this.Car = car;
        }

        public IGroupMold Owner { get; }
        public Atom Car { get; }
        public string Name { get; set; }
        public virtual IDictionary<string, object> KeywordValues { get; } = new Dictionary<string, object>();
    }
}
