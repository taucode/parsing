﻿using NUnit.Framework;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispLexerTests
    {
        [Test]
        public void Lexize_OnlyComments_EmptyOutput()
        {
            // Arrange
            var input =
                @"
; first comment
; second comment";

            // Act
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(0)); // comments will not be added as tokens.
        }

        [Test]
        public void Lexize_HasComments_OmitsComments()
        {
            // Arrange
            var input = @"();wat";

            // Act
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(2));

            Assert.That(tokens[0] as PunctuationToken, Has.Property("Value").EqualTo(Punctuation.LeftParenthesis));
            Assert.That(tokens[1] as PunctuationToken, Has.Property("Value").EqualTo(Punctuation.RightParenthesis));
        }

        [Test]
        public void Lexize_ComplexInput_ProducesValidResult()
        {
            // Arrange
            var input =
                @"
; CREATE
(:defblock create
    (:word ""CREATE"")
    (:alt (:block create-table) (:block create-index))
)
";

            // Act
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(19));

            Assert.That(
                tokens[0] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[1] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":defblock"));
            Assert.That(tokens[2] as LispSymbolToken, Has.Property(nameof(LispSymbolToken.Symbol)).EqualTo("create"));

            Assert.That(
                tokens[3] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[4] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":word"));

            Assert.That(tokens[5] as StringToken, Has.Property(nameof(StringToken.Value)).EqualTo("CREATE"));

            Assert.That(
                tokens[6] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[7] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[8] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":alt"));

            Assert.That(
                tokens[9] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[10] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":block"));

            Assert.That(
                tokens[11] as LispSymbolToken,
                Has.Property(nameof(LispSymbolToken.Symbol)).EqualTo("create-table"));

            Assert.That(
                tokens[12] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[13] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[14] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":block"));

            Assert.That(
                tokens[15] as LispSymbolToken,
                Has.Property(nameof(LispSymbolToken.Symbol)).EqualTo("create-index"));

            Assert.That(
                tokens[16] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[17] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[18] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));
        }

        [Test]
        public void Lexize_SqlGrammar_LexizesCorrectly()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);

            // Act
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(input);

            // Assert
            // passed
        }
    }
}
