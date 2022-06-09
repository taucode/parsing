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

        protected override void ValidateResult(Element element, IScriptElementMold scriptElementMold)
        {
            var partMold = (IPartMold)scriptElementMold;

            var groupMold = (GroupMold)partMold;

            if (groupMold.Content.Count == 0)
            {
                throw new NotImplementedException("error todo: group cannot be empty, check it in base class");
            }


            // find first (0th) part that is the actual start of the list // todo: provide a pic here
            var startingPartIndex = -1;

            for (var i = 0; i < groupMold.Content.Count; i++)
            {
                var innerScriptElementMold = groupMold.Content[i];
                if (innerScriptElementMold is IPartMold)
                {
                    startingPartIndex = i;
                    break;
                }
            }

            if (startingPartIndex == -1)
            {
                throw new NotImplementedException(
                    "error todo: group must contain at least one real part (or even vertex)?");
            }

            var startingPart = (IPartMold)groupMold.Content[startingPartIndex];
            if (startingPart.IsEntrance || startingPart.IsExit)
            {
                throw new NotImplementedException(
                    "error: elements of sequence must not have explicit isEntrance and isExit");
            }

            groupMold.Entrance = startingPart.Entrance;
            
            // todo clean
            //var index = 0;
            var currentHead = startingPart;

            for (var i = startingPartIndex + 1; i < groupMold.Content.Count; i++)
            {
                var innerScriptElementMold = groupMold.Content[i];
                if (innerScriptElementMold is PartMoldBase nextPartMold)
                {
                    // let's work
                }
                else
                {
                    if (innerScriptElementMold is IPartMold)
                    {
                        throw new NotImplementedException("error: IPartMold, but not PartMoldBase");
                    }

                    continue; // nothing to validate here, it must be IArc.
                }

                if (nextPartMold.IsEntrance || nextPartMold.IsExit)
                {
                    throw new NotImplementedException(
                        "error: elements of sequence must not have explicit isEntrance and isExit");
                }

                if (currentHead.Exit == null)
                {
                    throw new NotImplementedException("error: cannot build sequence: no exit in prev part");
                }

                if (nextPartMold.Entrance == null)
                {
                    throw new NotImplementedException("error: cannot build sequence: no exit in prev part");
                }

                currentHead.Exit.AddLinkTo(nextPartMold.Entrance);

                currentHead = nextPartMold;
            }

            groupMold.Exit = currentHead.Exit;


            // build the sequence

            //throw new NotImplementedException();


            //for (var i = 0; i < groupMold.Content.Count; i++)
            //{
            //    var innerScriptElementMold = groupMold.Content[i];
            //    var innerPart = innerScriptElementMold as IPartMold;

            //    if (innerPart != null)
            //    {
            //        if (innerPart.IsEntrance)
            //        {
            //            throw new NotImplementedException();
            //        }

            //        if (innerPart.IsExit)
            //        {
            //            throw new NotImplementedException();
            //        }
            //    }


            //    if (i == 0)
            //    {
            //        if (innerPart != null)
            //        {
            //            groupMold.Entrance = innerPart.Entrance;
            //        }
            //    }

            //    if (i < groupMold.Content.Count - 1)
            //    {
            //        if (innerPart.Exit == null)
            //        {
            //            throw new NotImplementedException(); // 0..n-1 th elements must have exit if they want to form a sequence
            //        }

            //        var nextInnerPart = groupMold.Content[i + 1];
            //        if (nextInnerPart.Entrance == null)
            //        {
            //            throw new NotImplementedException();
            //        }

            //        innerPart.Exit.AddLinkTo(nextInnerPart.Entrance);
            //    }

            //    if (i == groupMold.Content.Count - 1)
            //    {
            //        groupMold.Exit = innerPart.Exit;
            //    }
            //}
        }
    }
}
