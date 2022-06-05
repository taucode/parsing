using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// todo regions
namespace Tau.Kappa.Parsing
{
    public class Lexer : ILexer
    {
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
                    throw Helper.CreateException(ParsingErrorTag.CannotTokenize, context.Position);
                }
            }

            return tokens;
        }

        public Action<LexingContext, ILexicalTokenProducer> OnBeforeTokenProduced { get; set; }

        public Action<LexingContext, ILexicalToken> OnAfterTokenProduced { get; set; }
    }
}
