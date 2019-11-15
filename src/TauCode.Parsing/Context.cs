﻿using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class Context : IContext
    {
        #region Fields

        private IReadOnlyCollection<INode> _nodes;

        #endregion

        #region Constructor

        public Context(ITokenStream tokenStream)
        {
            this.TokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
            this.ResultAccumulator = new ResultAccumulator();
        }

        #endregion

        #region Private



        #endregion

        #region IContext Members

        public ITokenStream TokenStream { get; }

        public void SetNodes(IReadOnlyCollection<INode> nodes)
        {
            // todo: checks

            _nodes = nodes;
        }

        public IReadOnlyCollection<INode> GetNodes() => _nodes;

        public IResultAccumulator ResultAccumulator { get; }

        #endregion
    }
}