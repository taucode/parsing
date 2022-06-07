using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.ElementReaders
{
    public class SequenceElementReader : GroupElementReader
    {
        public SequenceElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {   
        }

        protected override void ValidateResult(Element element, IPartMold partMold)
        {
            var groupMold = (IGroupMold)partMold;

            if (groupMold.Content.Count == 0)
            {
                throw new NotImplementedException();
            }

            for (var i = 0; i < groupMold.Content.Count; i++)
            {
                var innerPart = groupMold.Content[i];

                if (innerPart.IsEntrance)
                {
                    throw new NotImplementedException();
                }

                if (innerPart.IsExit)
                {
                    throw new NotImplementedException();
                }

                if (i < groupMold.Content.Count - 1)
                {
                    if (innerPart.Exit == null)
                    {
                        throw new NotImplementedException(); // 0..n-1 th elements must have exit if they want to form a sequence
                    }

                    var nextInnerPart = groupMold.Content[i + 1];
                    if (nextInnerPart.Entrance == null)
                    {
                        throw new NotImplementedException();
                    }

                    innerPart.Exit.AddLinkTo(nextInnerPart.Entrance);
                }
            }
        }
    }
}
