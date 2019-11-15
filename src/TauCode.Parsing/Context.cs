﻿using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class Context : IContext
    {
        private IReadOnlyCollection<INode> _nodes;

        public Context(ITokenStream tokenStream)
        {
            this.TokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
            this.ResultAccumulator = new ResultAccumulator();
        }

        public ITokenStream TokenStream { get; }

        public void SetNodes(IReadOnlyCollection<INode> nodes)
        {
            // todo: checks

            _nodes = nodes;
        }

        public IReadOnlyCollection<INode> GetNodes() => _nodes;

        public IResultAccumulator ResultAccumulator { get; }
    }
}
