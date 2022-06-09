using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    // todo clean, regions
    public class SplitterElementReader : GroupElementReader
    {
        public SplitterElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void ValidateResult(Element element, IScriptElementMold scriptElementMold)
        {
            var groupMold = (GroupMold)scriptElementMold;

            var innerParts = groupMold
                .Content
                .Where(x => x is IPartMold)
                .Cast<PartMoldBase>() // todo can throw
                .ToList();

            if (innerParts.Count < 2)
            {
                throw new NotImplementedException("error: splitter must have at lease two nodes: root + some other");
            }

            if (!(innerParts[0] is VertexMold splitterRoot))
            {
                throw new NotImplementedException("error: first element in splitter must be a vertex");
            }

            if (!splitterRoot.IsEntrance)
            {
                throw new NotImplementedException("error: first element in splitter must be entrance.");
            }

            var choiceCount = innerParts.Count - 1; // emitting root

            var last = innerParts[^1];
            IVertexMold exitVertex = null;

            if (last.IsExit)
            {
                if (innerParts.Count < 3)
                {
                    throw new NotImplementedException();
                }

                if (last is IVertexMold lastVertex)
                {
                    exitVertex = lastVertex;
                    choiceCount--;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            for (var i = 0; i < choiceCount; i++)
            {
                var index = i + 1;
                var innerPart = innerParts[index];

                if (innerPart.IsEntrance)
                {
                    throw new NotImplementedException();
                }

                if (innerPart.IsExit)
                {
                    throw new NotImplementedException();
                }

                if (innerPart.Entrance == null)
                {
                    throw new NotImplementedException();
                }

                splitterRoot.AddLinkTo(innerPart.Entrance);

                if (exitVertex != null)
                {
                    if (innerPart.Exit == null)
                    {
                        throw new NotImplementedException();
                    }

                    innerPart.Exit.AddLinkTo(exitVertex);
                }
            }

            groupMold.Entrance = splitterRoot;
            groupMold.Exit = exitVertex;
        }
    }
}
