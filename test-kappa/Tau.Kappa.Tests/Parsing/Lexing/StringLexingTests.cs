using System;
using System.Linq;
using NUnit.Framework;
using Tau.Kappa.Parsing;
using Tau.Kappa.Parsing.Exceptions;
using Tau.Kappa.Parsing.LexicalTokenProducers;
using Tau.Kappa.Parsing.LexicalTokens;

namespace Tau.Kappa.Tests.Parsing.Lexing
{
    [TestFixture]
    public class StringLexingTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new Lexer();
            _lexer.Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new CLangStringProducer(),
            };
        }

        [Test]
        public void EscapeString_SingleCharEscape_EscapesCorrectly()
        {
            // Arrange
            var input = "\"\\n\\v\\t\\r\\a\\b\\0\\t\\f\\\\\\\"\"";

            // Act
            var tokens = _lexer.Tokenize(input.AsMemory());

            // Assert
            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n\v\t\r\a\b\0\t\f\\\""));
        }

        [Test]
        public void EscapeString_MixedEscapes_EscapesCorrectly()
        {
            // Arrange
            var input = "\"\\n\\v\\t\\r\\a\\b\\0\\t\\f\\\\\\u1488\"";

            // Act
            var tokens = _lexer.Tokenize(input.AsMemory());

            // Assert
            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n\v\t\r\a\b\0\t\f\\\u1488"));
        }

        [Test]
        public void Tokenize_UnclosedString_ThrowsLexingException()
        {
            // Arrange
            var input = " \"Unclosed string";

            // Act
            var ex = Assert.Throws<ParsingException>(() => _lexer.Tokenize(input.AsMemory()));
            
            // Assert
            Assert.That(ex.Message, Does.StartWith("Unclosed string."));
            Assert.That(ex.Index, Is.EqualTo(17));
        }

        [Test]
        [TestCase("\"Broken string\n")]
        [TestCase("\"Broken string\r")]
        [TestCase("\"Broken string\n\r")]
        [TestCase("\"Broken string\r\n")]
        public void Tokenize_NewLineInString_ThrowsLexingException(string input)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ParsingException>(() => _lexer.Tokenize(input.AsMemory()));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Newline in string."));
            Assert.That(ex.Index, Is.EqualTo(14));
        }

        [Test]
        [TestCase("\"End after escape\\")]
        public void Tokenize_EndAfterEscape_ThrowsLexingException(string input)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ParsingException>(() => _lexer.Tokenize(input.AsMemory()));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Unclosed string."));
            Assert.That(ex.Index, Is.EqualTo(18));
        }

        [Test]
        [TestCase("\"Bad\\o\"")]
        public void Tokenize_WrongSingleCharEscape_ThrowsLexingException(string input)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ParsingException>(() => _lexer.Tokenize(input.AsMemory()));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Bad escape sequence."));
            Assert.That(ex.Index, Is.EqualTo(4));
        }

        [Test]
        [TestCase("\"Bad\\u")]
        [TestCase("\"Bad\\u1")]
        [TestCase("\"Bad\\u11")]
        [TestCase("\"Bad\\u111")]
        [TestCase("\"Bad\\u111z\"")]
        public void Tokenize_BadUEscape_ThrowsLexingException(string input)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ParsingException>(() => _lexer.Tokenize(input.AsMemory()));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Bad escape sequence."));
            Assert.That(ex.Index, Is.EqualTo(4));
        }
    }
}
