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

        public virtual IList<ILexicalToken> Tokenize(ReadOnlyMemory<char> input)
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
                var gotEmptySpaceSkipped = false;

                foreach (var producer in this.Producers)
                {
                    this.OnBeforeTokenProduced?.Invoke(context, producer);

                    var positionBeforeProduce = context.Position;

                    if (this.IgnoreEmptyTokens && producer is IEmptyLexicalTokenProducer emptyLexicalTokenProducer)
                    {
                        var emptySpaceSkipped = emptyLexicalTokenProducer.Skip(context); // todo: ut that empty token producer doesn't 
                        if (emptySpaceSkipped > 0)
                        {
                            if (context.Position + emptySpaceSkipped > context.Input.Length)
                            {
                                throw new LexingException($"Empty token producer has skipped too much input. Type: '{producer.GetType().FullName}'.");
                            }
                            else
                            {
                                context.Position += emptySpaceSkipped;
                                gotEmptySpaceSkipped = true;
                                break;
                            }
                        }
                    }

                    var token = producer.Produce(context);
                    if (positionBeforeProduce != context.Position)
                    {
                        throw new LexingException($"Token producer has changed context position. Type: '{producer.GetType().FullName}'.");
                    }

                    if (token == null)
                    {
                        // this producer failed.
                    }
                    else
                    {
                        if (context.Position + token.ConsumedLength > context.Input.Length)
                        {
                            throw new LexingException($"Token producer has consumed too much input. Type: '{producer.GetType().FullName}'.");
                        }

                        this.OnAfterTokenProduced?.Invoke(context, token);
                        gotToken = true;

                        var ignoreToken = token is IEmptyLexicalToken && this.IgnoreEmptyTokens;
                        if (!ignoreToken)
                        {
                            tokens.Add(token);
                        }

                        context.Position += token.ConsumedLength;

                        break;
                    }
                }

                if (gotToken || gotEmptySpaceSkipped)
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
