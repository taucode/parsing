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

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            //base.ReadContent(scriptElementMold, element);

            var groupMold = (GroupMold)scriptElementMold;
            var parts = groupMold.Linkables;

            if (parts.Count < 2)
            {
                throw new NotImplementedException("error: too little vertices for splitter.");
            }

            var splittingVertex = parts[0] as VertexMold;
            if (splittingVertex == null)
            {
                throw new NotImplementedException("error: zeroth part must be vertex");
            }

            if (!splittingVertex.IsEntrance)
            {
                throw new NotImplementedException("error: zeroth part must be entrance");
            }

            var jointPart = parts.SingleOrDefault(x => x.GetKeywordValueAsBool(":IS-JOINT"));
            if (jointPart == null || jointPart is VertexMold)
            {
                // that's ok
            }
            else
            {
                throw new NotImplementedException("error: joint must be a vertex.");
            }

            var joint = (VertexMold)jointPart;
            if (joint == splittingVertex)
            {
                throw new NotImplementedException("error: splitting vertex and joint vertex cannot be the same");
            }

            if (joint != null)
            {
                if (parts.Count < 3)
                {
                    throw new NotImplementedException("error: if both splitting and joint vertices present, count should be >=3");
                }
            }

            if (joint != null && joint != parts[^1])
            {
                throw new NotImplementedException("error: joint (if presents) must be the last vertex.");
            }

            var upper = parts.Count;
            if (joint != null)
            {
                upper--;
            }

            for (var i = 1; i < upper; i++)
            {
                var part = parts[i];
                splittingVertex.AddLinkTo(part.GetEntranceVertexOrThrow());

                if (joint != null)
                {
                    part.GetExitVertexOrThrow().AddLinkTo(joint);
                }
            }
        }

        //protected override void FinalizeMold(IScriptElementMold scriptElementMold, Element element)
        //{
        //    var groupMold = (GroupMold)scriptElementMold;

        //    throw new NotImplementedException();

        //    //var innerParts = groupMold
        //    //    .Content
        //    //    .Where(x => x is IPartMold)
        //    //    .Cast<PartMoldBase>() // todo can throw
        //    //    .ToList();

        //    //if (innerParts.Count < 2)
        //    //{
        //    //    throw new NotImplementedException("error: splitter must have at lease two nodes: root + some other");
        //    //}

        //    //if (!(innerParts[0] is VertexMold splitterRoot))
        //    //{
        //    //    throw new NotImplementedException("error: first element in splitter must be a vertex");
        //    //}

        //    //if (!splitterRoot.IsEntrance)
        //    //{
        //    //    throw new NotImplementedException("error: first element in splitter must be entrance.");
        //    //}

        //    //var choiceCount = innerParts.Count - 1; // emitting root

        //    //var last = innerParts[^1];
        //    //IVertexMold exitVertex = null;

        //    //if (last.IsExit)
        //    //{
        //    //    if (innerParts.Count < 3)
        //    //    {
        //    //        throw new NotImplementedException();
        //    //    }

        //    //    if (last is IVertexMold lastVertex)
        //    //    {
        //    //        exitVertex = lastVertex;
        //    //        choiceCount--;
        //    //    }
        //    //    else
        //    //    {
        //    //        throw new NotImplementedException();
        //    //    }
        //    //}

        //    //for (var i = 0; i < choiceCount; i++)
        //    //{
        //    //    var index = i + 1;
        //    //    var innerPart = innerParts[index];

        //    //    if (innerPart.IsEntrance)
        //    //    {
        //    //        throw new NotImplementedException();
        //    //    }

        //    //    if (innerPart.IsExit)
        //    //    {
        //    //        throw new NotImplementedException();
        //    //    }

        //    //    if (innerPart.Entrance == null)
        //    //    {
        //    //        throw new NotImplementedException();
        //    //    }

        //    //    splitterRoot.AddLinkTo(innerPart.Entrance);

        //    //    if (exitVertex != null)
        //    //    {
        //    //        if (innerPart.Exit == null)
        //    //        {
        //    //            throw new NotImplementedException();
        //    //        }

        //    //        innerPart.Exit.AddLinkTo(exitVertex);
        //    //    }
        //    //}

        //    //groupMold.Entrance = splitterRoot;
        //    //groupMold.Exit = exitVertex;
        //}
    }
}
