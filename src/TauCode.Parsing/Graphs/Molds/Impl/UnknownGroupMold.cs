using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    internal sealed class UnknownGroupMold : IGroupMold
    {
        internal static readonly IGroupMold Instance = new UnknownGroupMold();

        private UnknownGroupMold()
        {   
        }

        public IGroupMold Owner => throw new NotImplementedException("error: invalid operation");
        public Atom Car => Nil.Instance;

        public string Name
        {
            get => throw new NotImplementedException("error: invalid operation");
            set => throw new NotImplementedException("error: invalid operation");
        }
        public IDictionary<string, object> KeywordValues => throw new NotImplementedException("error: invalid operation");
        public string GetFullPath() => throw new NotImplementedException("error: invalid operation");

        public bool IsEntrance
        {
            get => throw new NotImplementedException("error: invalid operation");
            set => throw new NotImplementedException("error: invalid operation");
        }

        public bool IsExit
        {
            get => throw new NotImplementedException("error: invalid operation");
            set => throw new NotImplementedException("error: invalid operation");
        }

        public IVertexMold Entrance => throw new NotImplementedException("error: invalid operation");

        public IVertexMold Exit => throw new NotImplementedException("error: invalid operation");
        public IReadOnlyList<IScriptElementMold> Content => throw new NotImplementedException("error: invalid operation");
        public void Add(IScriptElementMold scriptElementMold) => throw new NotImplementedException("error: invalid operation");
    }
}
