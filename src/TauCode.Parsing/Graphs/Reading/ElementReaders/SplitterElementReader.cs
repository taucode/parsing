using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.ElementReaders
{
    public class SplitterElementReader : GroupElementReader
    {
        public SplitterElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void ValidateResult(Element element, IPartMold partMold)
        {
            throw new NotImplementedException();
        }
    }
}
