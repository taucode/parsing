﻿using System;
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
            var groupMold = (GroupMold)scriptElementMold;
            var linkables = groupMold.Linkables;

            if (linkables.Count < 2)
            {
                throw new NotImplementedException("error: too little vertices for splitter.");
            }

            var splittingVertex = linkables[0] as VertexMold;
            if (splittingVertex == null)
            {
                throw new NotImplementedException("error: zeroth linkable must be vertex");
            }

            if (!splittingVertex.IsEntrance)
            {
                throw new NotImplementedException("error: zeroth linkable must be entrance");
            }

            var jointLinkableMold = linkables.SingleOrDefault(x => x.GetKeywordValueAsBool(":IS-JOINT"));
            if (jointLinkableMold == null || jointLinkableMold is VertexMold)
            {
                // that's ok
            }
            else
            {
                throw new NotImplementedException("error: joint must be a vertex.");
            }

            var joint = (VertexMold)jointLinkableMold;
            if (joint == splittingVertex)
            {
                throw new NotImplementedException("error: splitting vertex and joint vertex cannot be the same");
            }

            if (joint != null)
            {
                if (linkables.Count < 3)
                {
                    throw new NotImplementedException("error: if both splitting and joint vertices present, count should be >=3");
                }
            }

            if (joint != null && joint != linkables[^1])
            {
                throw new NotImplementedException("error: joint (if presents) must be the last vertex.");
            }

            var upper = linkables.Count;
            if (joint != null)
            {
                upper--;
            }

            for (var i = 1; i < upper; i++)
            {
                var linkableMold = linkables[i];
                splittingVertex.AddLinkTo(linkableMold.GetEntranceVertexOrThrow());

                if (joint != null)
                {
                    linkableMold.GetExitVertexOrThrow().AddLinkTo(joint);
                }
            }
        }
    }
}