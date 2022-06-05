using System;
using System.Linq;
using NUnit.Framework;
using TauCode.Parsing.LexicalTokenProducers;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.Tests.Parsing.Cli;

[TestFixture]
public class CliLexerTests
{
    private ILexer _lexer;

    [SetUp]
    public void SetUp()
    {
        _lexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new CliWordProducer(),
                new CliKeyProducer(),
                new IntegerProducer(),
                new JsonStringProducer(),
                new FilePathProducer(),
            }
        };
    }

    [Test]
    public void Tokenize_ValidInput_ProducesValidTokens()
    {
        // Arrange
        var input = "pub -t one '{\"name\" : \"ak\"}' --repeat 88 -log c:/temp/logs --level ba-c";

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(10));

        // pub
        var termToken = (TextToken)tokens[0];
        Assert.That(termToken, Is.TypeOf<CliWordToken>());
        Assert.That(termToken.Text, Is.EqualTo("pub"));
        Assert.That(termToken.Position, Is.EqualTo(0));
        Assert.That(termToken.ConsumedLength, Is.EqualTo(3));

        // -t
        var keyToken = (TextToken)tokens[1];
        Assert.That(keyToken, Is.TypeOf<CliKeyToken>());
        Assert.That(keyToken.Text, Is.EqualTo("-t"));
        Assert.That(keyToken.Position, Is.EqualTo(4));
        Assert.That(keyToken.ConsumedLength, Is.EqualTo(2));

        // one
        termToken = (TextToken)tokens[2];
        Assert.That(termToken, Is.TypeOf<CliWordToken>());
        Assert.That(termToken.Text, Is.EqualTo("one"));
        Assert.That(termToken.Position, Is.EqualTo(7));
        Assert.That(termToken.ConsumedLength, Is.EqualTo(3));

        // '{\"name\" : \"ak\"}'
        var stringToken = (TextToken)tokens[3];
        Assert.That(stringToken, Is.TypeOf<StringToken>());
        Assert.That(stringToken.Text, Is.EqualTo("{\"name\" : \"ak\"}"));
        Assert.That(stringToken.Position, Is.EqualTo(11));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(17));

        // --repeat
        keyToken = (TextToken)tokens[4];
        Assert.That(keyToken, Is.TypeOf<CliKeyToken>());
        Assert.That(keyToken.Text, Is.EqualTo("--repeat"));
        Assert.That(keyToken.Position, Is.EqualTo(29));
        Assert.That(keyToken.ConsumedLength, Is.EqualTo(8));

        // 88
        var intToken = (IntegerToken)tokens[5];
        Assert.That(intToken.Value, Is.EqualTo(88));
        Assert.That(intToken.Position, Is.EqualTo(38));
        Assert.That(intToken.ConsumedLength, Is.EqualTo(2));

        // -log
        keyToken = (TextToken)tokens[6];
        Assert.That(keyToken, Is.TypeOf<CliKeyToken>());
        Assert.That(keyToken.Text, Is.EqualTo("-log"));
        Assert.That(keyToken.Position, Is.EqualTo(41));
        Assert.That(keyToken.ConsumedLength, Is.EqualTo(4));

        // c:/temp/logs
        var pathToken = (TextToken)tokens[7];
        Assert.That(pathToken, Is.TypeOf<FilePathToken>());
        Assert.That(pathToken.Text, Is.EqualTo("c:/temp/logs"));
        Assert.That(pathToken.Position, Is.EqualTo(46));
        Assert.That(pathToken.ConsumedLength, Is.EqualTo(12));

        // --level
        keyToken = (TextToken)tokens[8];
        Assert.That(keyToken, Is.TypeOf<CliKeyToken>());
        Assert.That(keyToken.Text, Is.EqualTo("--level"));
        Assert.That(keyToken.Position, Is.EqualTo(59));
        Assert.That(keyToken.ConsumedLength, Is.EqualTo(7));

        // 1a-c
        termToken = (TextToken)tokens[9];
        Assert.That(termToken, Is.TypeOf<CliWordToken>());
        Assert.That(termToken.Text, Is.EqualTo("ba-c"));
        Assert.That(termToken.Position, Is.EqualTo(67));
        Assert.That(termToken.ConsumedLength, Is.EqualTo(4));
    }

    [Test]
    [TestCase("-a-b")]
    public void Tokenize_KeyWithHyphen_TokenizesCorrectly(string input)
    {
        // Arrange

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(1));
        var textToken = (CliKeyToken)tokens.Single();
        Assert.That(textToken.Text, Is.EqualTo("-a-b"));
    }

    [Test]
    [TestCase(".")]
    [TestCase("..")]
    [TestCase("-")]
    [TestCase("--")]
    [TestCase("---")]
    [TestCase("--fo-")]
    [TestCase("-fo-")]
    [TestCase("---foo")]
    public void Tokenize_StrangePath_TokenizesAsPath(string input)
    {
        // Arrange

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(1));
        var textToken = (FilePathToken)tokens.Single();
        Assert.That(textToken.Text, Is.EqualTo(input));
    }
}