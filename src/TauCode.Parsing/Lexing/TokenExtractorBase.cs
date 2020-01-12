﻿using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public abstract class TokenExtractorBase<TToken> : ITokenExtractor<TToken>
        where TToken : IToken
    {
        #region Nested

        protected enum CharAcceptanceResult
        {
            Continue = 1,
            Stop,
            Fail,
        }

        #endregion

        #region Fields

        private bool _isProcessing;

        #endregion

        #region Abstract

        protected abstract CharAcceptanceResult AcceptCharImpl(char c, int localIndex);

        protected abstract bool AcceptsPreviousTokenImpl(IToken previousToken);

        protected abstract void OnBeforeProcess();

        #endregion

        #region Protected
        
        protected ILexingContext Context { get; private set; }

        protected virtual bool ProcessEnd()
        {
            // idle, no problem for most token extractors.
            //
            // but of course will fail for unclosed strings, SQL identifiers [my_column_name , etc.
            // in such a case, throw an 'Unexpected-end' LexingException here.

            return true;
        }

        protected virtual TextProcessingResult Subprocess()
        {
            return TextProcessingResult.Fail;
        }

        protected Position StartPosition { get; private set; }

        protected CharAcceptanceResult ContinueOrFail(bool b)
        {
            // todo: take adv. of it anywhere
            return b ? CharAcceptanceResult.Continue : CharAcceptanceResult.Fail;
        }

        protected virtual bool IsProducer =>
            true; // most token extractors produce something; however, comment extractors do not.

        #endregion

        #region ITokenExtractor<TToken> Members

        public abstract TToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength);

        #endregion

        #region ITextProcessor<TProduct> Members

        public bool AcceptsFirstChar(char c)
        {
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            var charAcceptanceResult = this.AcceptCharImpl(c, 0);
            switch (charAcceptanceResult)
            {
                case CharAcceptanceResult.Continue:
                    return true;

                case CharAcceptanceResult.Stop:
                    throw new NotImplementedException(); // error in your logic.

                case CharAcceptanceResult.Fail:
                    return false;

                default:
                    throw new NotImplementedException(); // how can be?
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                if (value == _isProcessing)
                {
                    throw new NotImplementedException(); // todo suspicious: why set to same value?
                }

                _isProcessing = value;
            }
        }

        public TextProcessingResult Process(ITextProcessingContext context)
        {
            // todo: prohibit recursion of 'Process()'
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // todo: temp (?) redundant check
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            var lexingContext = (ILexingContext)context;

            var previousChar = lexingContext.TryGetPreviousLocalChar();
            if (previousChar.HasValue && !LexingHelper.IsInlineWhiteSpaceOrCaretControl(previousChar.Value))
            {
                var previousToken = lexingContext.Tokens.LastOrDefault(); // todo: DO optimize.
                if (previousToken != null)
                {
                    var acceptsPreviousToken = this.AcceptsPreviousTokenImpl(previousToken);

                    if (!acceptsPreviousToken)
                    {
                        return TextProcessingResult.Fail;
                    }
                }
            }

            this.Context = lexingContext;
            this.StartPosition = this.Context.GetCurrentAbsolutePosition();
            this.Context.RequestGeneration();

            this.Context.AdvanceByChar(); // since 'Process' has been called, it means that 'First' (i.e. 0th) char was accepted by Lexer.

            this.OnBeforeProcess();
            this.IsProcessing = true;

            var gotStop = false;

            while (true)
            {
                var isEnd = this.Context.IsEnd();

                if (isEnd || gotStop)
                {
                    if (this.Context.GetLocalIndex() == 0)
                    {
                        throw new NotImplementedException(); // todo: how on earth did we get here?
                    }

                    if (isEnd && !gotStop)
                    {
                        var acceptsEnd = this.ProcessEnd(); // throw here if you need.
                        if (!acceptsEnd)
                        {
                            this.Context.ReleaseGeneration();
                            this.IsProcessing = false;
                            return TextProcessingResult.Fail;
                        }
                    }

                    var summary = this.IsProducer ? TextProcessingSummary.CanProduce : TextProcessingSummary.Skip;

                    var myAbsoluteIndex = this.Context.GetAbsoluteIndex();
                    var myLine = this.Context.GetCurrentLine();
                    var currentColumn = this.Context.GetCurrentColumn();

                    this.Context.ReleaseGeneration();

                    var oldAbsoluteIndex = this.Context.GetAbsoluteIndex();
                    var oldLine = this.Context.GetCurrentLine();

                    var indexShift = myAbsoluteIndex - oldAbsoluteIndex;
                    var lineShift = myLine - oldLine;

                    this.IsProcessing = false;
                    return new TextProcessingResult(summary, indexShift, lineShift, currentColumn);
                }

                var delegatedResult = this.Subprocess();
                if (delegatedResult.Summary != TextProcessingSummary.Fail)
                {
                    throw new NotImplementedException();
                }

                var c = this.Context.GetCurrentChar();
                var localIndex = this.Context.GetLocalIndex();

                var oldContextVersion = this.Context.Version;
                var acceptanceResult = this.AcceptCharImpl(c, localIndex);

                // check.
                if (this.Context.GetLocalIndex() == 0 && !acceptanceResult.IsIn(CharAcceptanceResult.Continue, CharAcceptanceResult.Fail))
                {
                    throw new NotImplementedException(); // todo error in your logic.
                }

                // check: only 'Stop' allows altering of context's version.
                if (acceptanceResult != CharAcceptanceResult.Stop)
                {
                    var newContextVersion = this.Context.Version;
                    if (oldContextVersion != newContextVersion)
                    {
                        throw new NotImplementedException();
                    }
                }

                switch (acceptanceResult)
                {
                    case CharAcceptanceResult.Continue:
                        this.Context.AdvanceByChar();
                        break;

                    case CharAcceptanceResult.Stop:
                        gotStop = true;
                        break;

                    case CharAcceptanceResult.Fail:
                        this.Context.ReleaseGeneration();
                        this.IsProcessing = false;
                        return TextProcessingResult.Fail;

                    default:
                        throw new NotImplementedException(); // wtf? (todo)
                }
            }
        }

        public IToken Produce(string text, int absoluteIndex, Position position, int consumedLength)
            => this.ProduceToken(text, absoluteIndex, position, consumedLength);

        #endregion

        #region Alpha Debug

        protected void AlphaCheckOnBeforeProcess()
        {
            var bad1 = this.IsProcessing;
            var bad2 = this.Context.GetLocalIndex() != 1;
            var good = !(bad1 || bad2);

            ParsingHelper.AlphaAssert(good);
        }

        #endregion
    }
}
