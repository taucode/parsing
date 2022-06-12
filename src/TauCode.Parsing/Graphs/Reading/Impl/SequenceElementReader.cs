using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class SequenceElementReader : GroupElementReader
    {
        public SequenceElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
        {
            base.ReadContent(scriptElementMold, element);

            var groupMold = (GroupMold)scriptElementMold;
            var zeroth = groupMold.Linkables[0];
            var last = groupMold.Linkables[^1];

            if (!zeroth.IsEntrance)
            {
                throw new NotImplementedException("error: 0th element of sequence must be entrance.");
            }

            if (!last.IsExit)
            {
                throw new NotImplementedException("error: last element of sequence must be exit.");
            }
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            //base.ReadContent(scriptElementMold, element);

            // todo: next block must be extracted into a brand new method 'CustomizeContent' or something.
            var group = (GroupMold)scriptElementMold;

            var linkables = group.Linkables;
            if (linkables.Count == 0)
            {
                throw new NotImplementedException("empty list");
            }

            for (var i = 0; i < linkables.Count; i++)
            {
                var part = linkables[i];

                if (i < linkables.Count - 1)
                {
                    var nextPart = linkables[i + 1];
                    part.GetExitVertexOrThrow().AddLinkTo(nextPart.GetEntranceVertexOrThrow());
                }
            }
        }

        //protected override void FinalizeMold(IScriptElementMold scriptElementMold, Element element)
        //{
        //    //var partMold = (IPartMold)scriptElementMold;

        //    //var groupMold = (GroupMold)partMold;

        //    //if (groupMold.AllElements.Count == 0)
        //    //{
        //    //    throw new NotImplementedException("error todo: group cannot be empty, check it in base class");
        //    //}


        //    //// find first (0th) part that is the actual start of the list // todo: provide a pic here
        //    //var startingPartIndex = -1;

        //    //for (var i = 0; i < groupMold.AllElements.Count; i++)
        //    //{
        //    //    var innerScriptElementMold = groupMold.AllElements[i];
        //    //    if (innerScriptElementMold is IPartMold)
        //    //    {
        //    //        startingPartIndex = i;
        //    //        break;
        //    //    }
        //    //}

        //    //if (startingPartIndex == -1)
        //    //{
        //    //    throw new NotImplementedException(
        //    //        "error todo: group must contain at least one real part (or even vertex)?");
        //    //}

        //    //var startingPart = (IPartMold)groupMold.AllElements[startingPartIndex];
        //    //if (startingPart.IsEntrance || startingPart.IsExit)
        //    //{
        //    //    throw new NotImplementedException(
        //    //        "error: elements of sequence must not have explicit isEntrance and isExit");
        //    //}

        //    //groupMold.Entrance = startingPart.Entrance;

        //    //// todo clean
        //    ////var index = 0;
        //    //var currentHead = startingPart;

        //    //for (var i = startingPartIndex + 1; i < groupMold.AllElements.Count; i++)
        //    //{
        //    //    var innerScriptElementMold = groupMold.AllElements[i];
        //    //    if (innerScriptElementMold is PartMoldBase nextPartMold)
        //    //    {
        //    //        // let's work
        //    //    }
        //    //    else
        //    //    {
        //    //        if (innerScriptElementMold is IPartMold)
        //    //        {
        //    //            throw new NotImplementedException("error: IPartMold, but not PartMoldBase");
        //    //        }

        //    //        continue; // nothing to validate here, it must be IArc.
        //    //    }

        //    //    if (nextPartMold.GetIsEntrance() || nextPartMold.GetIsExit())
        //    //    {
        //    //        throw new NotImplementedException(
        //    //            "error: elements of sequence must not have explicit isEntrance and isExit");
        //    //    }

        //    //    if (currentHead.GetExitVertex() == null)
        //    //    {
        //    //        throw new NotImplementedException("error: cannot build sequence: no exit in prev part");
        //    //    }

        //    //    if (nextPartMold.GetEntranceVertex() == null)
        //    //    {
        //    //        throw new NotImplementedException("error: cannot build sequence: no exit in prev part");
        //    //    }

        //    //    currentHead.GetExitVertex().AddLinkTo(nextPartMold.GetEntranceVertex());

        //    //    currentHead = nextPartMold;
        //    //}

        //    //groupMold.Exit = currentHead.Exit;


        //    //========================================================

        //    // build the sequence

        //    //throw new NotImplementedException();


        //    //for (var i = 0; i < groupMold.Content.Count; i++)
        //    //{
        //    //    var innerScriptElementMold = groupMold.Content[i];
        //    //    var innerPart = innerScriptElementMold as IPartMold;

        //    //    if (innerPart != null)
        //    //    {
        //    //        if (innerPart.IsEntrance)
        //    //        {
        //    //            throw new NotImplementedException();
        //    //        }

        //    //        if (innerPart.IsExit)
        //    //        {
        //    //            throw new NotImplementedException();
        //    //        }
        //    //    }


        //    //    if (i == 0)
        //    //    {
        //    //        if (innerPart != null)
        //    //        {
        //    //            groupMold.Entrance = innerPart.Entrance;
        //    //        }
        //    //    }

        //    //    if (i < groupMold.Content.Count - 1)
        //    //    {
        //    //        if (innerPart.Exit == null)
        //    //        {
        //    //            throw new NotImplementedException(); // 0..n-1 th elements must have exit if they want to form a sequence
        //    //        }

        //    //        var nextInnerPart = groupMold.Content[i + 1];
        //    //        if (nextInnerPart.Entrance == null)
        //    //        {
        //    //            throw new NotImplementedException();
        //    //        }

        //    //        innerPart.Exit.AddLinkTo(nextInnerPart.Entrance);
        //    //    }

        //    //    if (i == groupMold.Content.Count - 1)
        //    //    {
        //    //        groupMold.Exit = innerPart.Exit;
        //    //    }
        //    //}
        //}
    }
}
