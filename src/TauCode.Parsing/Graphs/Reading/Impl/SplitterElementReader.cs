using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class SplitterElementReader : GroupElementReader
    {
        public SplitterElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void ValidateResult(Element element, IPartMold partMold)
        {
            var groupMold = (GroupMold)partMold;

            if (groupMold.Content.Count < 2)
            {
                throw new NotImplementedException(); // splitter must have at lease two nodes: root + some other
            }

            if (!(groupMold.Content[0] is VertexMold splitterRoot))
            {
                throw new NotImplementedException();
            }

            if (!splitterRoot.IsEntrance)
            {
                throw new NotImplementedException();
            }

            var choiceCount = groupMold.Content.Count - 1; // emitting root

            var last = groupMold.Content[^1];
            IVertexMold exitVertex = null;

            if (last.IsExit)
            {
                if (groupMold.Content.Count < 3)
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
                var innerPart = groupMold.Content[index];

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
