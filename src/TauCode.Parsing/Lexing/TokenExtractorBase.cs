﻿using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    // todo clean up
    public abstract class TokenExtractorBase : ITokenExtractor
    {
        #region Fields

        private string _input;
        //private int _startPos;
        //private int _localPos;
        private readonly List<ITokenExtractor> _successors;

        #endregion

        #region Constructor

        protected TokenExtractorBase(
            //ILexingEnvironment environment,
            Func<char, bool> firstCharPredicate)
        {

            //Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            FirstCharPredicate = firstCharPredicate ?? throw new ArgumentNullException(nameof(firstCharPredicate));

            _successors = new List<ITokenExtractor>();
        }

        #endregion

        #region Abstract

        protected abstract void ResetState();

        protected abstract IToken ProduceResult();

        protected abstract CharChallengeResult ChallengeCurrentChar();

        protected abstract CharChallengeResult ChallengeEnd();

        #endregion

        #region Protected

        protected int StartingAbsoluteCharIndex { get; private set; }

        protected int StartingLine { get; private set; }
        protected int LineShift { get; private set; }

        protected int StartingColumn { get; private set; }
        protected int CurrentColumn { get; private set; }

        protected int LocalCharIndex { get; private set; }

        protected Func<char, bool> FirstCharPredicate { get; }


        protected bool IsEnd() => this.StartingAbsoluteCharIndex + this.LocalCharIndex == _input.Length;

        //protected bool IsEnd() => this.GetAbsolutePosition() == _input.Length;

        //protected int GetLocalCharIndex() => _localPos;

        //protected int GetAbsolutePosition() => _startPos + _localPos;
        protected int GetAbsolutePosition() => this.StartingAbsoluteCharIndex + this.LocalCharIndex;

        //protected char GetLocalChar(int localPosition)
        //{
        //    return _input[_startPos + localPosition];
        //}

        protected virtual bool AllowsCharAfterProduction(char c)
        {
            foreach (var successor in _successors)
            {
                if (successor.AllowsFirstChar(c))
                {
                    return true;
                }
            }

            return false;
        }

        protected string ExtractResultString()
        {
            //var str = _input.Substring(_startPos, _localPos);
            var str = _input.Substring(this.StartingAbsoluteCharIndex, this.LocalCharIndex);
            return str;
        }

        protected void Advance()
        {
            this.LocalCharIndex++;
            this.CurrentColumn++;

            //_localPos++;
        }

        protected void SkipSingleLineBreak()
        {
            var c = this.GetCurrentChar();
            int indexShift;
            
            if (c == '\r') // todo constant
            {
                var nextChar = this.GetNextChar();
                if (nextChar.HasValue)
                {
                    if (nextChar.Value == '\n')
                    {
                        indexShift = 2; // got CRLF
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (c == '\n') // todo constant
            {
                throw new NotImplementedException();
            }
            else
            {
                // how on earth did we get here?
                throw LexingHelper.CreateInternalErrorException(); // todo: 'CreateInternalError_Lexing_Exception'
            }

            this.LocalCharIndex += indexShift;
            this.LineShift++;
            this.CurrentColumn = 0;
        }

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateInternalErrorException();
            }

            var absPos = this.GetAbsolutePosition();
            var c = _input[absPos];
            return c;
        }

        protected char? GetNextChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateInternalErrorException();
            }

            var absIndex = this.StartingAbsoluteCharIndex + this.LocalCharIndex + 1;
            if (absIndex == _input.Length)
            {
                return null;
            }

            var c = _input[absIndex];
            return c;
        }

        public void AddSuccessors(params TokenExtractorBase[] successors) => _successors.AddRange(successors);

        #endregion

        #region ITokenExtractor Members

        public TokenExtractionResult Extract(string input, int charIndex, int line, int column)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));

            if (charIndex < 0 || charIndex >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(charIndex));
            }

            //_startPos = position;
            //_localPos = 0;

            this.StartingAbsoluteCharIndex = charIndex;

            this.StartingLine = line;
            this.LineShift = 0;

            this.StartingColumn = column;
            this.CurrentColumn = column;

            this.LocalCharIndex = 0;

            this.ResetState();

            while (true)
            {
                if (this.IsEnd())
                {
                    var challengeEnd = this.ChallengeEnd();

                    switch (challengeEnd)
                    {
                        case CharChallengeResult.Finish:
                            var token = this.ProduceResult();
                            if (token == null)
                            {
                                // possible situation. e.g. in LISP '+1488' looks like as a symbol at the beginning, but at the end would appear
                                // an integer, and symbol extractor would refuse deliver such a result as a symbol.
                                throw new NotImplementedException();
                                //return new TokenExtractionResult(0, null);
                            }
                            else
                            {
                                return new TokenExtractionResult(token, this.LocalCharIndex, this.LineShift, this.CurrentColumn);
                                //return new TokenExtractionResult(this.GetLocalPosition(), token);
                            }

                        case CharChallengeResult.GiveUp:
                            throw new NotImplementedException();
                        //return new TokenExtractionResult(0, null);

                        case CharChallengeResult.Error:
                            throw new LexingException("Unexpected end of input.");

                        default:
                            throw LexingHelper.CreateInternalErrorException();
                    }
                }

                var testCharResult = this.ChallengeCurrentChar();

                switch (testCharResult)
                {
                    case CharChallengeResult.GiveUp:
                        throw new NotImplementedException();
                    //return new TokenExtractionResult(0, null); // this extractor failed to recognize the whole token, no problem.

                    case CharChallengeResult.Continue:
                        this.Advance(); // todo: deal with line breaks?
                        break;

                    case CharChallengeResult.Finish:
                        var token = this.ProduceResult();

                        if (token == null)
                        {
                            return new TokenExtractionResult(null, 0, 0, null);
                        }

                        // check if next char is ok.
                        if (!this.IsEnd())
                        {
                            var upcomingChar = this.GetCurrentChar();
                            //if (!Environment.IsSpace(upcomingChar))
                            if (!LexingHelper.IsInlineWhiteSpaceOrCaretControl(upcomingChar))
                            {
                                var check = this.AllowsCharAfterProduction(upcomingChar);
                                if (!check)
                                {
                                    throw new LexingException($"Unexpected char: '{upcomingChar}'.");
                                }
                            }
                        }

                        //return new TokenExtractionResult(this.GetLocalPosition(), token);
                        return new TokenExtractionResult(token, this.LocalCharIndex, this.LineShift, this.CurrentColumn);

                    default:
                        throw new LexingException($"Internal error. Unexpected test char result: '{testCharResult}'.");
                }
            }
        }

        public virtual bool AllowsFirstChar(char c) => FirstCharPredicate(c);

        #endregion
    }
}
