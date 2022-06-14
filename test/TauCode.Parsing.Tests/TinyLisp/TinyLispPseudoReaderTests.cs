using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispPseudoReaderTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new TinyLispLexer();
        }

        [Test]
        public void Read_SqlGrammar_ProducesExpectedResult()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            

            var tokens = _lexer.Tokenize(input.AsMemory());

            var reader = new TinyLispPseudoReader();

            // Act
            var list = reader.Read(tokens);

            // Assert
            Assert.That(list, Has.Count.EqualTo(10));

            var expectedTexts = this.GetType().Assembly
                .GetResourceText("sql-grammar-expected.lisp", true)
                .Split(";;; splitting comment", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            Assert.That(expectedTexts, Has.Count.EqualTo(list.Count()));

            for (int i = 0; i < list.Count; i++)
            {
                var actual = list[i].ToString();

                var alteredActual = actual
                    .Replace(" )", ")")
                    .Replace(" )", ")")
                    .Replace(" (", "(")
                    .Replace(" (", "(");

                var expected = expectedTexts[i]
                    .Replace(Environment.NewLine, " ")
                    .Replace("\t", "")
                    .Replace(" )", ")")
                    .Replace(" )", ")")
                    .Replace(" (", "(")
                    .Replace(" (", "(");

                Assert.That(alteredActual, Is.EqualTo(expected).IgnoreCase);
            }
        }

        [Test]
        public void Read_UnclosedForm_ThrowsTinyLispException()
        {
            // Arrange
            var form = "(un-closed (a (bit))";
            
            var tokens = _lexer.Tokenize(form.AsMemory());
            var reader = new TinyLispPseudoReader();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Unclosed form."));
            Assert.That(ex.Index, Is.EqualTo(20));
        }

        [Test]
        public void Read_ExtraRightParenthesis_ThrowsTinyLispException()
        {
            // Arrange
            var form = "(closed too much))";
            
            var tokens = _lexer.Tokenize(form.AsMemory());
            var reader = new TinyLispPseudoReader();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Unexpected ')'."));
            Assert.That(ex.Index, Is.EqualTo(17));
        }

        [Test]
        public void Read_UnsupportedToken_ThrowsTinyLispException()
        {
            // Arrange
            var form = "(some good form)";
            
            var tokens = _lexer.Tokenize(form.AsMemory());
            var list = tokens.ToList();

            var badToken = new EnumToken<int>(0, 4, 1488);
            list.Insert(1, badToken);
            var reader = new TinyLispPseudoReader();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => reader.Read(list));

            // Assert
            Assert.That(ex.Message, Does.StartWith("Cannot read token."));
        }
    }
}
