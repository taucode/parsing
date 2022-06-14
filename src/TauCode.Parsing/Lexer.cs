using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing
{
    public class Lexer : ILexer
    {
        #region ILexer Members

        public IEnumerable<ILexicalTokenProducer> Producers { get; set; }

        public bool IgnoreEmptyTokens { get; set; } = true;

        public virtual IReadOnlyList<ILexicalToken> Tokenize(ReadOnlyMemory<char> input)
        {
            var gotProducers = this.Producers?.Any() ?? false;
            if (!gotProducers)
            {
                throw new InvalidOperationException(
                    $"'{nameof(Producers)}' must be initialized with non-empty collection.");
            }

            var context = new LexingContext(input);
            var tokens = new List<ILexicalToken>();

            var length = input.Length;
            while (context.Position < length)
            {
                var gotToken = false;

                foreach (var producer in this.Producers)
                {
                    this.OnBeforeTokenProduced?.Invoke(context, producer);

                    var positionBeforeProduce = context.Position;

                    var token = producer.Produce(context);
                    if (token == null)
                    {
                        if (positionBeforeProduce != context.Position)
                        {
                            throw new LexingException(
                                "Internal error: token producer returned 'null', but context position has advanced.",
                                positionBeforeProduce);
                        }

                        // do nothing
                    }
                    else
                    {
                        this.OnAfterTokenProduced?.Invoke(context, token);

                        gotToken = true;

                        if (token is IEmptyLexicalToken && this.IgnoreEmptyTokens)
                        {
                            // do nothing
                        }
                        else
                        {
                            if (context.Position == positionBeforeProduce)
                            {
                                throw new NotImplementedException();
                            }

                            tokens.Add(token);
                        }

                        break;
                    }
                }

                if (gotToken)
                {

                    // do nothing
                }
                else
                {
                    throw LexingHelper.CreateException(LexingErrorTag.CannotTokenize, context.Position);
                }
            }

            return tokens;
        }

        public Action<LexingContext, ILexicalTokenProducer> OnBeforeTokenProduced { get; set; }

        public Action<LexingContext, ILexicalToken> OnAfterTokenProduced { get; set; }

        #endregion
    }
}
