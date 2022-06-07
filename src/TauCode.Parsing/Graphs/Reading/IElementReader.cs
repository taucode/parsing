using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading
{
    public interface IElementReader
    {
        IPartMold Read(IPartMold owner, Element element);
    }
}
