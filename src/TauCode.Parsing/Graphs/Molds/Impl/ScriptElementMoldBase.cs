using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Extensions;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo regions
    public abstract class ScriptElementMoldBase : IScriptElementMold
    {
        private readonly Dictionary<string, object> _keywordValues;
        private string _name;

        protected ScriptElementMoldBase(IGroupMold owner, Atom car)
        {
            this.Owner = owner;
            this.Car = car;
            _keywordValues = new Dictionary<string, object>();
        }

        public IGroupMold Owner { get; }
        public Atom Car { get; }

        public string Name
        {
            get => _name;
            set
            {
                this.CheckNotFinalized();
                _name = value;
            }
        }

        public Element LispElement { get; internal set; }

        public void SetKeywordValue(string keyword, object value)
        {
            this.CheckNotFinalized();

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

        protected void CheckNotFinalized()
        {
            if (this.IsFinalized)
            {
                throw new NotImplementedException("error: finalized.");
            }
        }

        protected void CheckFinalized()
        {
            if (!this.IsFinalized)
            {
                throw new NotImplementedException("error: not finalized.");
            }
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
            this.CheckNotFinalized();

            if (keyword == null)
            {
                throw new ArgumentNullException(nameof(keyword));
            }

            return _keywordValues.Remove(keyword);
        }

        public virtual void ProcessKeywords()
        {
            this.CheckNotFinalized();

            this.Name = (string)this.GetKeywordValue(":NAME");
        }

        public bool IsFinalized { get; private set; }
        public void ValidateAndFinalize()
        {
            this.CheckNotFinalized();

            this.ValidateAndFinalizeImpl();

            this.IsFinalized = true;
        }

        protected virtual void ValidateAndFinalizeImpl()
        {
            //_name = (string)this.GetKeywordValue(":NAME");
        }
    }
}
