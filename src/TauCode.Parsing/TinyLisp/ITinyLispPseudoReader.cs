﻿using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.TinyLisp
{
    public interface ITinyLispPseudoReader
    {
        PseudoList Read(IReadOnlyList<ILexicalToken> tokens);
    }
}
