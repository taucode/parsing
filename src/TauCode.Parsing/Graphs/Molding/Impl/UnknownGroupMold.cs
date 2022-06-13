using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    internal sealed class UnknownGroupMold : IGroupMold
    {
        #region Singleton

        internal static readonly IGroupMold Instance = new UnknownGroupMold();

        private UnknownGroupMold()
        {
        }

        #endregion

        #region IScriptElementMold Members

        public IGroupMold Owner => throw CreateInvalidOperationException();

        public Atom Car => throw CreateInvalidOperationException();

        public string Name
        {
            get => throw CreateInvalidOperationException();
            set => throw CreateInvalidOperationException();
        }

        public Element LispElement => throw CreateInvalidOperationException();

        public void SetKeywordValue(string keyword, object value) => throw CreateInvalidOperationException();

        public object GetKeywordValue(string keyword) => throw CreateInvalidOperationException();

        public IReadOnlyCollection<string> Keywords => throw CreateInvalidOperationException();

        public bool RemoveKeyword(string keyword) => throw CreateInvalidOperationException();

        public void ProcessKeywords() => throw CreateInvalidOperationException();

        public bool IsFinalized => throw CreateInvalidOperationException();

        public void ValidateAndFinalize() => throw CreateInvalidOperationException();

        #endregion

        #region ILinkableMold Members

        public string GetFullPath() => throw CreateInvalidOperationException();

        public bool IsEntrance
        {
            get => throw CreateInvalidOperationException();
            set => throw CreateInvalidOperationException();
        }

        public bool IsExit
        {
            get => throw CreateInvalidOperationException();
            set => throw CreateInvalidOperationException();
        }

        public IVertexMold GetEntranceVertex() => throw CreateInvalidOperationException();

        public IVertexMold GetExitVertex() => throw CreateInvalidOperationException();

        #endregion

        #region IGroupMold Members

        public IReadOnlyList<IScriptElementMold> AllElements => throw CreateInvalidOperationException();

        public IReadOnlyList<ILinkableMold> Linkables => throw CreateInvalidOperationException();

        public void Add(IScriptElementMold scriptElementMold) => throw CreateInvalidOperationException();

        #endregion

        #region Privte

        private static Exception CreateInvalidOperationException()
        {
            return new InvalidOperationException($"Methods of the '{nameof(UnknownGroupMold)}' should never be called.");
        }

        #endregion
    }
}
