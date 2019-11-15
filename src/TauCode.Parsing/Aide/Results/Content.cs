﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class Content : IContent
    {
        #region Fields

        private readonly List<IAideResult> _results;
        private bool _isSealed;

        #endregion

        #region Constructor

        public Content(IAideResult owner)
        {
            // todo checks
            this.Owner = owner;
            _results = new List<IAideResult>();
        }

        #endregion

        #region Private

        private void CheckNotSealed()
        {
            if (_isSealed)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IReadOnlyList<IAideResult> Members

        public IEnumerator<IAideResult> GetEnumerator() => _results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _results.GetEnumerator();

        public int Count => _results.Count;

        public IAideResult this[int index] => _results[index];

        #endregion

        #region IContent Members

        public IAideResult Owner { get; }

        public void AddResult(IAideResult result)
        {
            // todo checks
            this.CheckNotSealed();

            _results.Add(result);
        }

        public void Seal()
        {
            this.CheckNotSealed();
            _isSealed = true;
        }

        public bool IsSealed => _isSealed;

        #endregion
    }
}
