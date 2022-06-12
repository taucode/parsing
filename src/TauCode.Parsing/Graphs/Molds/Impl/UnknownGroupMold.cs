using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo clean
    internal sealed class UnknownGroupMold : IGroupMold
    {
        internal static readonly IGroupMold Instance = new UnknownGroupMold();

        private UnknownGroupMold()
        {   
        }

        public IGroupMold Owner => throw new NotImplementedException("error: invalid operation");
        public Atom Car => throw new NotImplementedException("error: invalid operation");

        public string Name
        {
            get => throw new NotImplementedException("error: invalid operation");
            set => throw new NotImplementedException("error: invalid operation");
        }

        public Element LispElement => throw new NotImplementedException("error: invalid operation");

        public void SetKeywordValue(string keyword, object value)
        {
            throw new NotImplementedException();
        }

        public object GetKeywordValue(string keyword)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<string> Keywords => throw new NotImplementedException("error: invalid operation");
        public bool RemoveKeyword(string keyword)
        {
            throw new NotImplementedException();
        }

        public void ProcessKeywords()
        {
            throw new NotImplementedException();
        }

        public bool IsFinalized => throw new NotImplementedException("error: invalid operation");
        public void ValidateAndFinalize()
        {
            throw new NotImplementedException();
        }


        public string GetFullPath() => throw new NotImplementedException("error: invalid operation");
        public bool IsEntrance { get; set; }
        public bool IsExit { get; set; }


        public IVertexMold GetEntranceVertex() => throw new NotImplementedException("error: invalid operation");

        public IVertexMold GetExitVertex() => throw new NotImplementedException("error: invalid operation");

        //public bool IsEntrance
        //{
        //    get => throw new NotImplementedException("error: invalid operation");
        //    set => throw new NotImplementedException("error: invalid operation");
        //}

        //public bool IsExit
        //{
        //    get => throw new NotImplementedException("error: invalid operation");
        //    set => throw new NotImplementedException("error: invalid operation");
        //}

        //public IVertexMold Entrance => throw new NotImplementedException("error: invalid operation");

        //public IVertexMold Exit => throw new NotImplementedException("error: invalid operation");

        public IReadOnlyList<IScriptElementMold> AllElements => throw new NotImplementedException("error: invalid operation");
        public IReadOnlyList<ILinkableMold> Linkables => throw new NotImplementedException("error: invalid operation");
        public void Add(IScriptElementMold scriptElementMold) => throw new NotImplementedException("error: invalid operation");
    }
}
