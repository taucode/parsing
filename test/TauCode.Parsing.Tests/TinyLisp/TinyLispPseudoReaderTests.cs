using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.TinyLisp;

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