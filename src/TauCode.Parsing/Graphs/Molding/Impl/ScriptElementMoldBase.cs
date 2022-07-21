using System;
using System.Collections.Generic;
using TauCode.Extensions;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    // todo clean
    public abstract class ScriptElementMoldBase : IScriptElementMold
    {
        #region Fields

        private readonly Dictionary<string, object> _keywordValues;

        #endregion

        #region ctor

        protected ScriptElementMoldBase(IGroupMold owner, Atom car)
        {
            this.Owner = owner;
            this.Car = car;
            _keywordValues = new Dictionary<string, object>();
        }

        #endregion

        #region IScriptElementMold Members

        public IGroupMold Owner { get; }

        public Atom Car { get; }

        public string Name { get; set; }

        public Element LispElement { get; internal set; } // todo: get rid of.

        public void SetKeywordValue(string keyword, object value)
        {
            if (keyword == null)
            {
                throw new ArgumentNullException(nameof(keyword));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.GetType().IsIn(
                    typeof(bool),
                    typeof(string),
                    typeof(int),
                    typeof(long),
                    typeof(double),
                    typeof(List<string>),
                    typeof(Dictionary<string, string>)
                ))
            {
                // ok
            }
            else
            {
                throw new NotImplementedException("todo: wrong type");
            }

            _keywordValues[keyword] = value;
        }

        public object GetKeywordValue(string keyword)
        {
            if (keyword == null)
            {
                throw new ArgumentNullException(nameof(keyword));
            }

            var exists = _keywordValues.TryGetValue(keyword, out var value);

            return exists ? value : null;
        }

        public IReadOnlyCollection<string> Keywords => _keywordValues.Keys;

        public bool RemoveKeyword(string keyword)
        {
            //this.CheckNotFinalized();

            if (keyword == null)
            {
                throw new ArgumentNullException(nameof(keyword));
            }

            return _keywordValues.Remove(keyword);
        }

        public virtual void ProcessKeywords()
        {
            //this.CheckNotFinalized();

            this.Name = this.GetKeywordValue<string>(":NAME", null);
        }

        #endregion
    }
}
