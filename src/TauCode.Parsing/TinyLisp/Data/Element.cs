﻿using System;

namespace TauCode.Parsing.TinyLisp.Data
{
    public abstract class Element : IEquatable<Element>
    {
        public abstract bool Equals(Element other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Element)obj);
        }

        protected abstract int GetHashCodeImpl();

        public override int GetHashCode() => this.GetHashCodeImpl();

        public static bool operator ==(Element a, Element b) => Equals(a, b);

        public static bool operator !=(Element a, Element b) => !Equals(a, b);
    }
}
