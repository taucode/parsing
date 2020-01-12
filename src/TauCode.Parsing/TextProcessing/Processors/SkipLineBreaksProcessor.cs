﻿using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing.Processors
{
    public class SkipLineBreaksProcessor : TextProcessorBase<string> // todo: Nothing.
    {
        private readonly bool _skipOnlyOneResult;

        public SkipLineBreaksProcessor(bool skipOnlyOneResult)
        {
            _skipOnlyOneResult = skipOnlyOneResult;
        }

        public override bool AcceptsFirstChar(char c)
        {
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            return LexingHelper.IsCaretControl(c);
        }

        public override TextProcessingResult Process(ITextProcessingContext context)
        {
            context.RequestGeneration();
            var goOn = true;

            while (goOn)
            {
                if (context.IsEnd())
                {
                    break;
                }

                var c = context.GetCurrentChar();

                switch (c)
                {
                    case LexingHelper.Cr:
                        if (_skipOnlyOneResult)
                        {
                            // whatever outcome is, stop processing.
                            goOn = false;
                        }

                        var nextChar = context.TryGetNextLocalChar();
                        if (nextChar.HasValue)
                        {
                            if (nextChar.Value == LexingHelper.Lf)
                            {
                                // got CRLF
                                context.Advance(2, 1, 0);
                                break;
                            }
                            else
                            {
                                // got CR and non-line-control-char
                                goOn = false; // redundant, but saves ~3 processor ticks :)
                                context.Advance(1, 1, 0);
                                break;
                            }
                        }
                        else
                        {
                            // end of input.
                            context.Advance(1, 1, 0);
                            break;
                        }

                    case '\n':
                        if (_skipOnlyOneResult)
                        {
                            // whatever outcome is, stop processing.
                            goOn = false;
                        }

                        context.Advance(1, 1, 0);
                        break;

                    default:
                        goOn = false;
                        break;
                }
            }

            context.ReleaseGenerationAndGetMetrics(out var indexShift, out var lineShift, out var currentColumn);
            return new TextProcessingResult(TextProcessingSummary.Skip, indexShift, lineShift, currentColumn);
        }

        public override string Produce(string text, int absoluteIndex, Position position, int consumedLength)
        {
            throw new NotImplementedException(); // todo should never be called
        }
    }
}