using NUnit.Framework;
using System;
using TauCode.Data.Text;
using TauCode.Extensions;
using TauCode.Parsing.TokenProducers;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.TokenProducers;

public enum Color
{
    White,
    Black,
}

[TestFixture]
public class TokenProducerTests
{
    [Test]
    public void TokenProducers_ValidInput_ProduceExpectedResultsVol1()
    {
        // Arrange
        var input = this.GetType().Assembly.GetResourceLines("TokenProducerTests.Sample001.txt", true)[0];

        ILexer lexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new Int32Producer(Helper.IsWhiteSpace),
                new Int64Producer(Helper.IsWhiteSpace),
                new DecimalProducer(Helper.IsWhiteSpace),
                new DoubleProducer(Helper.IsWhiteSpace),
                new FilePathProducer(Helper.IsWhiteSpace),
                new UriProducer(Helper.IsWhiteSpace),
            },
            IgnoreEmptyTokens = false,
        };

        // Act
        var tokens = lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(17));

        // 1
        var int32Token = (Int32Token)tokens[0];
        Assert.That(int32Token.Value, Is.EqualTo(1));
        Assert.That(int32Token.Text, Is.EqualTo("1"));
        Assert.That(int32Token.Position, Is.EqualTo(0));
        Assert.That(int32Token.ConsumedLength, Is.EqualTo(1));

        // <space>
        var emptyToken = (EmptyToken)tokens[1];
        Assert.That(emptyToken.Position, Is.EqualTo(1));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(3));

        // 9223372036854775807
        var int64Token = (Int64Token)tokens[2];
        Assert.That(int64Token.Value, Is.EqualTo(9223372036854775807));
        Assert.That(int64Token.Text, Is.EqualTo("9223372036854775807"));
        Assert.That(int64Token.Position, Is.EqualTo(4));
        Assert.That(int64Token.ConsumedLength, Is.EqualTo(19));

        // <space>
        emptyToken = (EmptyToken)tokens[3];
        Assert.That(emptyToken.Position, Is.EqualTo(23));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(4));

        // 9223372036854775808
        var decimalToken = (DecimalToken)tokens[4];
        Assert.That(decimalToken.Value, Is.EqualTo(9223372036854775808m));
        Assert.That(decimalToken.Text, Is.EqualTo("9223372036854775808"));
        Assert.That(decimalToken.Position, Is.EqualTo(27));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(19));

        // <space>
        emptyToken = (EmptyToken)tokens[5];
        Assert.That(emptyToken.Position, Is.EqualTo(46));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(11));

        // 79228162514264337593543950335
        decimalToken = (DecimalToken)tokens[6];
        Assert.That(decimalToken.Value, Is.EqualTo(79228162514264337593543950335m));
        Assert.That(decimalToken.Text, Is.EqualTo("79228162514264337593543950335"));
        Assert.That(decimalToken.Position, Is.EqualTo(57));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(29));

        // <space>
        emptyToken = (EmptyToken)tokens[7];
        Assert.That(emptyToken.Position, Is.EqualTo(86));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(6));

        // 79228162514264337593543950336
        var filePathToken = (FilePathToken)tokens[8];
        Assert.That(filePathToken.Text, Is.EqualTo("79228162514264337593543950336"));
        Assert.That(filePathToken.Position, Is.EqualTo(92));
        Assert.That(filePathToken.ConsumedLength, Is.EqualTo(29));

        // <space>
        emptyToken = (EmptyToken)tokens[9];
        Assert.That(emptyToken.Position, Is.EqualTo(121));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(4));

        // https://a.me
        var uriToken = (UriToken)tokens[10];
        Assert.That(uriToken.Value, Is.EqualTo(new Uri("https://a.me/")));
        Assert.That(uriToken.Text, Is.EqualTo("https://a.me/"));
        Assert.That(uriToken.Position, Is.EqualTo(125));
        Assert.That(uriToken.ConsumedLength, Is.EqualTo(12));

        // <space>
        emptyToken = (EmptyToken)tokens[11];
        Assert.That(emptyToken.Position, Is.EqualTo(137));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(9));

        // c:\temp\v1
        filePathToken = (FilePathToken)tokens[12];
        Assert.That(filePathToken.Text, Is.EqualTo(@"c:\temp\v1"));
        Assert.That(filePathToken.Position, Is.EqualTo(146));
        Assert.That(filePathToken.ConsumedLength, Is.EqualTo(10));

        // <space>
        emptyToken = (EmptyToken)tokens[13];
        Assert.That(emptyToken.Position, Is.EqualTo(156));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(6));

        // -1.2
        decimalToken = (DecimalToken)tokens[14];
        Assert.That(decimalToken.Value, Is.EqualTo(-1.2m));
        Assert.That(decimalToken.Text, Is.EqualTo("-1.2"));
        Assert.That(decimalToken.Position, Is.EqualTo(162));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(4));

        // <space>
        emptyToken = (EmptyToken)tokens[15];
        Assert.That(emptyToken.Position, Is.EqualTo(166));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(6));

        // -1.2
        var doubleToken = (DoubleToken)tokens[16];
        Assert.That(doubleToken.Value, Is.EqualTo(-1.2));
        Assert.That(doubleToken.Text, Is.EqualTo("-1.2"));
        Assert.That(doubleToken.Position, Is.EqualTo(172));
        Assert.That(doubleToken.ConsumedLength, Is.EqualTo(6));

    }

    [Test]
    public void TokenProducers_ValidInput_ProduceExpectedResultsVol2()
    {
        // Arrange
        var input = this.GetType().Assembly.GetResourceLines("TokenProducerTests.Sample001.txt", true)[0];

        ILexer lexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new Int32Producer(Helper.IsWhiteSpace),
                new Int64Producer(Helper.IsWhiteSpace),
                new DecimalProducer(Helper.IsWhiteSpace),
                new DoubleProducer(Helper.IsWhiteSpace),
                new FilePathProducer(Helper.IsWhiteSpace),
                new UriProducer(Helper.IsWhiteSpace),
            },
            IgnoreEmptyTokens = false,
        };

        // Act
        var tokens = lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(17));

        // 1
        var int32Token = (Int32Token)tokens[0];
        Assert.That(int32Token.Value, Is.EqualTo(1));
        Assert.That(int32Token.Text, Is.EqualTo("1"));
        Assert.That(int32Token.Position, Is.EqualTo(0));
        Assert.That(int32Token.ConsumedLength, Is.EqualTo(1));

        // <space>
        var emptyToken = (EmptyToken)tokens[1];
        Assert.That(emptyToken.Position, Is.EqualTo(1));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(3));

        // 9223372036854775807
        var int64Token = (Int64Token)tokens[2];
        Assert.That(int64Token.Value, Is.EqualTo(9223372036854775807));
        Assert.That(int64Token.Text, Is.EqualTo("9223372036854775807"));
        Assert.That(int64Token.Position, Is.EqualTo(4));
        Assert.That(int64Token.ConsumedLength, Is.EqualTo(19));

        // <space>
        emptyToken = (EmptyToken)tokens[3];
        Assert.That(emptyToken.Position, Is.EqualTo(23));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(4));

        // 9223372036854775808
        var decimalToken = (DecimalToken)tokens[4];
        Assert.That(decimalToken.Value, Is.EqualTo(9223372036854775808m));
        Assert.That(decimalToken.Text, Is.EqualTo("9223372036854775808"));
        Assert.That(decimalToken.Position, Is.EqualTo(27));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(19));

        // <space>
        emptyToken = (EmptyToken)tokens[5];
        Assert.That(emptyToken.Position, Is.EqualTo(46));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(11));

        // 79228162514264337593543950335
        decimalToken = (DecimalToken)tokens[6];
        Assert.That(decimalToken.Value, Is.EqualTo(79228162514264337593543950335m));
        Assert.That(decimalToken.Text, Is.EqualTo("79228162514264337593543950335"));
        Assert.That(decimalToken.Position, Is.EqualTo(57));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(29));

        // <space>
        emptyToken = (EmptyToken)tokens[7];
        Assert.That(emptyToken.Position, Is.EqualTo(86));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(6));

        // 79228162514264337593543950336
        var filePathToken = (FilePathToken)tokens[8];
        Assert.That(filePathToken.Text, Is.EqualTo("79228162514264337593543950336"));
        Assert.That(filePathToken.Position, Is.EqualTo(92));
        Assert.That(filePathToken.ConsumedLength, Is.EqualTo(29));

        // <space>
        emptyToken = (EmptyToken)tokens[9];
        Assert.That(emptyToken.Position, Is.EqualTo(121));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(4));

        // https://a.me
        var uriToken = (UriToken)tokens[10];
        Assert.That(uriToken.Value, Is.EqualTo(new Uri("https://a.me/")));
        Assert.That(uriToken.Text, Is.EqualTo("https://a.me/"));
        Assert.That(uriToken.Position, Is.EqualTo(125));
        Assert.That(uriToken.ConsumedLength, Is.EqualTo(12));

        // <space>
        emptyToken = (EmptyToken)tokens[11];
        Assert.That(emptyToken.Position, Is.EqualTo(137));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(9));

        // c:\temp\v1
        filePathToken = (FilePathToken)tokens[12];
        Assert.That(filePathToken.Text, Is.EqualTo(@"c:\temp\v1"));
        Assert.That(filePathToken.Position, Is.EqualTo(146));
        Assert.That(filePathToken.ConsumedLength, Is.EqualTo(10));

        // <space>
        emptyToken = (EmptyToken)tokens[13];
        Assert.That(emptyToken.Position, Is.EqualTo(156));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(6));

        // -1.2
        decimalToken = (DecimalToken)tokens[14];
        Assert.That(decimalToken.Value, Is.EqualTo(-1.2m));
        Assert.That(decimalToken.Text, Is.EqualTo("-1.2"));
        Assert.That(decimalToken.Position, Is.EqualTo(162));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(4));

        // <space>
        emptyToken = (EmptyToken)tokens[15];
        Assert.That(emptyToken.Position, Is.EqualTo(166));
        Assert.That(emptyToken.ConsumedLength, Is.EqualTo(6));

        // -1.2
        var doubleToken = (DoubleToken)tokens[16];
        Assert.That(doubleToken.Value, Is.EqualTo(-1.2));
        Assert.That(doubleToken.Text, Is.EqualTo("-1.2"));
        Assert.That(doubleToken.Position, Is.EqualTo(172));
        Assert.That(doubleToken.ConsumedLength, Is.EqualTo(6));

    }

    [Test]
    public void TokenProducers_ValidInput_ProduceExpectedResultsVol3()
    {
        // Arrange
        var input = this.GetType().Assembly.GetResourceLines("TokenProducerTests.Sample001.txt", true)[0];

        ILexer lexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new Int32Producer(Helper.IsWhiteSpace),
                new Int64Producer(Helper.IsWhiteSpace),
                new DecimalProducer(Helper.IsWhiteSpace),
                new DoubleProducer(Helper.IsWhiteSpace),
                new FilePathProducer(Helper.IsWhiteSpace),
                new UriProducer(Helper.IsWhiteSpace),
            },
            IgnoreEmptyTokens = true,
        };

        // Act
        var tokens = lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(9));

        // 1
        var int32Token = (Int32Token)tokens[0];
        Assert.That(int32Token.Value, Is.EqualTo(1));
        Assert.That(int32Token.Text, Is.EqualTo("1"));
        Assert.That(int32Token.Position, Is.EqualTo(0));
        Assert.That(int32Token.ConsumedLength, Is.EqualTo(1));

        // 9223372036854775807
        var int64Token = (Int64Token)tokens[1];
        Assert.That(int64Token.Value, Is.EqualTo(9223372036854775807));
        Assert.That(int64Token.Text, Is.EqualTo("9223372036854775807"));
        Assert.That(int64Token.Position, Is.EqualTo(4));
        Assert.That(int64Token.ConsumedLength, Is.EqualTo(19));

        // 9223372036854775808
        var decimalToken = (DecimalToken)tokens[2];
        Assert.That(decimalToken.Value, Is.EqualTo(9223372036854775808m));
        Assert.That(decimalToken.Text, Is.EqualTo("9223372036854775808"));
        Assert.That(decimalToken.Position, Is.EqualTo(27));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(19));

        // 79228162514264337593543950335
        decimalToken = (DecimalToken)tokens[3];
        Assert.That(decimalToken.Value, Is.EqualTo(79228162514264337593543950335m));
        Assert.That(decimalToken.Text, Is.EqualTo("79228162514264337593543950335"));
        Assert.That(decimalToken.Position, Is.EqualTo(57));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(29));

        // 79228162514264337593543950336
        var filePathToken = (FilePathToken)tokens[4];
        Assert.That(filePathToken.Text, Is.EqualTo("79228162514264337593543950336"));
        Assert.That(filePathToken.Position, Is.EqualTo(92));
        Assert.That(filePathToken.ConsumedLength, Is.EqualTo(29));

        // https://a.me
        var uriToken = (UriToken)tokens[5];
        Assert.That(uriToken.Value, Is.EqualTo(new Uri("https://a.me/")));
        Assert.That(uriToken.Text, Is.EqualTo("https://a.me/"));
        Assert.That(uriToken.Position, Is.EqualTo(125));
        Assert.That(uriToken.ConsumedLength, Is.EqualTo(12));

        // c:\temp\v1
        filePathToken = (FilePathToken)tokens[6];
        Assert.That(filePathToken.Text, Is.EqualTo(@"c:\temp\v1"));
        Assert.That(filePathToken.Position, Is.EqualTo(146));
        Assert.That(filePathToken.ConsumedLength, Is.EqualTo(10));

        // -1.2
        decimalToken = (DecimalToken)tokens[7];
        Assert.That(decimalToken.Value, Is.EqualTo(-1.2m));
        Assert.That(decimalToken.Text, Is.EqualTo("-1.2"));
        Assert.That(decimalToken.Position, Is.EqualTo(162));
        Assert.That(decimalToken.ConsumedLength, Is.EqualTo(4));

        // -1.2
        var doubleToken = (DoubleToken)tokens[8];
        Assert.That(doubleToken.Value, Is.EqualTo(-1.2));
        Assert.That(doubleToken.Text, Is.EqualTo("-1.2"));
        Assert.That(doubleToken.Position, Is.EqualTo(172));
        Assert.That(doubleToken.ConsumedLength, Is.EqualTo(6));
    }

    [Test]
    public void TokenProducers_ValidInput_ProduceExpectedResultsVol4()
    {
        // Arrange
        var input = this.GetType().Assembly.GetResourceLines("TokenProducerTests.Sample002.txt", true)[0];

        ILexer lexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new BooleanProducer(Helper.IsWhiteSpace),
                new WhiteSpaceProducer(),
                new EnumProducer<Color>(terminator: Helper.IsWhiteSpace),
                new KeyProducer(Helper.IsWhiteSpace),
                new TermProducer(Helper.IsWhiteSpace),
                new SemanticVersionProducer(Helper.IsWhiteSpace),
                new HostNameProducer(Helper.IsWhiteSpace),
                new EmailAddressProducer(Helper.IsWhiteSpace),
                new JsonStringProducer(Helper.IsWhiteSpace),
            },
            IgnoreEmptyTokens = true,
        };

        // Act
        var tokens = lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(8));

        // true
        var booleanToken = (BooleanToken)tokens[0];
        Assert.That(booleanToken.Value, Is.EqualTo(true));
        Assert.That(booleanToken.Text, Is.EqualTo("true"));
        Assert.That(booleanToken.Position, Is.EqualTo(0));
        Assert.That(booleanToken.ConsumedLength, Is.EqualTo(4));

        // White
        var enumColorToken = (EnumToken<Color>)tokens[1];
        Assert.That(enumColorToken.Value, Is.EqualTo(Color.White));
        Assert.That(enumColorToken.Text, Is.EqualTo("White"));
        Assert.That(enumColorToken.Position, Is.EqualTo(10));
        Assert.That(enumColorToken.ConsumedLength, Is.EqualTo(5));

        // korba@mail.net
        var emailAddressToken = (EmailAddressToken)tokens[2];
        Assert.That(emailAddressToken.Value, Is.EqualTo(EmailAddress.Parse("korba@mail.net")));
        Assert.That(emailAddressToken.Text, Is.EqualTo("korba@mail.net"));
        Assert.That(emailAddressToken.Position, Is.EqualTo(24));
        Assert.That(emailAddressToken.ConsumedLength, Is.EqualTo(14));

        // ::
        var hostNameToken = (HostNameToken)tokens[3];
        Assert.That(hostNameToken.Value, Is.EqualTo(HostName.Parse("::")));
        Assert.That(hostNameToken.Text, Is.EqualTo("::"));
        Assert.That(hostNameToken.Position, Is.EqualTo(40));
        Assert.That(hostNameToken.ConsumedLength, Is.EqualTo(2));

        // 1.0.2-rc3
        var semanticVersionToken = (SemanticVersionToken)tokens[4];
        Assert.That(semanticVersionToken.Value, Is.EqualTo(SemanticVersion.Parse("1.0.2-rc3")));
        Assert.That(semanticVersionToken.Text, Is.EqualTo("1.0.2-rc3"));
        Assert.That(semanticVersionToken.Position, Is.EqualTo(48));
        Assert.That(semanticVersionToken.ConsumedLength, Is.EqualTo(9));

        // "'\ud83d\udc08'\r\n"
        var stringToken = (StringToken)tokens[5];
        Assert.That(stringToken.Kind, Is.EqualTo("JSON"));
        Assert.That(stringToken.Text, Is.EqualTo("'🐈'\r\n"));
        Assert.That(stringToken.Position, Is.EqualTo(68));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(20));

        // --some-key
        var keyToken = (KeyToken)tokens[6];
        Assert.That(keyToken.Text, Is.EqualTo("--some-key"));
        Assert.That(keyToken.Position, Is.EqualTo(96));
        Assert.That(keyToken.ConsumedLength, Is.EqualTo(10));

        // some-term
        var termToken = (TermToken)tokens[7];
        Assert.That(termToken.Text, Is.EqualTo("some-term"));
        Assert.That(termToken.Position, Is.EqualTo(112));
        Assert.That(termToken.ConsumedLength, Is.EqualTo(9));
    }
}
