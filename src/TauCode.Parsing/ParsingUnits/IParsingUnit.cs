﻿using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public interface IParsingUnit
    {
        string Name { get; set; }
        IParsingBlock Owner { get; }
        bool IsFinalized { get; }
        void FinalizeUnit();
        IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context);
    }
}
