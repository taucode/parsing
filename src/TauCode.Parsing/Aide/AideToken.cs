﻿namespace TauCode.Parsing.Aide
{
    public abstract class AideToken : IToken
    {
        protected AideToken(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
