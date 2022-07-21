using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.TinyLisp;

[TestFixture]
public class TinyLispLexerTests
{
    private const string CR = "\r";
    private const string LF = "\n";
    private const string CRLF = CR + LF;
    private const string DQ = "\"";

    private ILexer _lexer;

    [SetUp]
    public void SetUp()
    {
        _lexer = new TinyLispLexer();
    }

    [Test]
    public void Tokenize_OnlyComments_EmptyOutput()
    {
        // Arrange
        var input =
            @"
; first comment
; second comment";

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(0)); // comments will not be added as tokens.
    }

    [Test]
    public void Tokenize_HasComments_OmitsComments()
    {
        // Arrange
        var input =
            @"();wat
-1599 -1599-";

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(4));

        var punctuationToken = (LispPunctuationToken)tokens[0];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(0));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        punctuationToken = (LispPunctuationToken)tokens[1];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(1));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));


        var integerToken = (Int32Token)tokens[2];
        Assert.That(integerToken.Value, Is.EqualTo(-1599));
        Assert.That(integerToken.Position, Is.EqualTo(8));
        Assert.That(integerToken.ConsumedLength, Is.EqualTo(5));

        var symbolToken = (LispSymbolToken)tokens[3];
        Assert.That(symbolToken.SymbolName, Is.EqualTo("-1599-"));
        Assert.That(symbolToken.Position, Is.EqualTo(14));
        Assert.That(symbolToken.ConsumedLength, Is.EqualTo(6));
    }

    [Test]
    public void Tokenize_ComplexInput_ProducesValidResult()
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
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(19));

        //  0: (
        var punctuationToken = (LispPunctuationToken)tokens[0];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(12));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        //  1: :defblock
        var keywordToken = (KeywordToken)tokens[1];
        Assert.That(keywordToken.Keyword, Is.EqualTo(":defblock"));
        Assert.That(keywordToken.Position, Is.EqualTo(13));
        Assert.That(keywordToken.ConsumedLength, Is.EqualTo(9));

        //  2: create
        var symbolToken = (LispSymbolToken)tokens[2];
        Assert.That(symbolToken.SymbolName, Is.EqualTo("create"));
        Assert.That(symbolToken.Position, Is.EqualTo(23));
        Assert.That(symbolToken.ConsumedLength, Is.EqualTo(6));

        //  3: (
        punctuationToken = (LispPunctuationToken)tokens[3];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(35));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        //  4: :word
        keywordToken = (KeywordToken)tokens[4];
        Assert.That(keywordToken.Keyword, Is.EqualTo(":word"));
        Assert.That(keywordToken.Position, Is.EqualTo(36));
        Assert.That(keywordToken.ConsumedLength, Is.EqualTo(5));

        //  5: "CREATE"
        var stringToken = (StringToken)tokens[5];
        Assert.That(stringToken.Text, Is.EqualTo("CREATE"));
        Assert.That(stringToken.Position, Is.EqualTo(42));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(8));

        //  6: )
        punctuationToken = (LispPunctuationToken)tokens[6];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(50));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        //  7: (
        punctuationToken = (LispPunctuationToken)tokens[7];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(57));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        //  8: :alt
        keywordToken = (KeywordToken)tokens[8];
        Assert.That(keywordToken.Keyword, Is.EqualTo(":alt"));
        Assert.That(keywordToken.Position, Is.EqualTo(58));
        Assert.That(keywordToken.ConsumedLength, Is.EqualTo(4));

        //  9: (
        punctuationToken = (LispPunctuationToken)tokens[9];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(63));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        // 10: :block
        keywordToken = (KeywordToken)tokens[10];
        Assert.That(keywordToken.Keyword, Is.EqualTo(":block"));
        Assert.That(keywordToken.Position, Is.EqualTo(64));
        Assert.That(keywordToken.ConsumedLength, Is.EqualTo(6));

        // 11: create-table
        symbolToken = (LispSymbolToken)tokens[11];
        Assert.That(symbolToken.SymbolName, Is.EqualTo("create-table"));
        Assert.That(symbolToken.Position, Is.EqualTo(71));
        Assert.That(symbolToken.ConsumedLength, Is.EqualTo(12));

        // 12: )
        punctuationToken = (LispPunctuationToken)tokens[12];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(83));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        // 13: (
        punctuationToken = (LispPunctuationToken)tokens[13];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(85));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        // 14: :block
        keywordToken = (KeywordToken)tokens[14];
        Assert.That(keywordToken.Keyword, Is.EqualTo(":block"));
        Assert.That(keywordToken.Position, Is.EqualTo(86));
        Assert.That(keywordToken.ConsumedLength, Is.EqualTo(6));

        // 15: create-index
        symbolToken = (LispSymbolToken)tokens[15];
        Assert.That(symbolToken.SymbolName, Is.EqualTo("create-index"));
        Assert.That(symbolToken.Position, Is.EqualTo(93));
        Assert.That(symbolToken.ConsumedLength, Is.EqualTo(12));

        // 16: )
        punctuationToken = (LispPunctuationToken)tokens[16];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(105));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        // 17: )
        punctuationToken = (LispPunctuationToken)tokens[17];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(106));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

        // 18: )
        punctuationToken = (LispPunctuationToken)tokens[18];
        Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
        Assert.That(punctuationToken.Position, Is.EqualTo(109));
        Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));
    }

    [Test]
    public void Tokenize_SimpleForm_TokenizesCorrectly()
    {
        // Arrange
        var input = "(a . b)";

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(5));
        Assert.That(tokens[0] as LispPunctuationToken, Has.Property(nameof(EnumToken<Punctuation>.Value)).EqualTo(Punctuation.LeftParenthesis));
        Assert.That(tokens[1] as LispSymbolToken, Has.Property(nameof(LispSymbolToken.SymbolName)).EqualTo("a"));
        Assert.That(tokens[2] as LispPunctuationToken, Has.Property(nameof(EnumToken<Punctuation>.Value)).EqualTo(Punctuation.Period));
        Assert.That(tokens[3] as LispSymbolToken, Has.Property(nameof(LispSymbolToken.SymbolName)).EqualTo("b"));
        Assert.That(tokens[4] as LispPunctuationToken, Has.Property(nameof(EnumToken<Punctuation>.Value)).EqualTo(Punctuation.RightParenthesis));
    }

    [Test]
    [TestCase("\r \n \r\n \n\r   \"not close", 5, 13, Description = "Not closed string 'not close'")]
    public void Tokenize_UnexpectedEnd_ThrowsLexerException(string notClosedString, int line, int column)
    {
        // Arrange
        var input = notClosedString;

        // Act
        var ex = Assert.Throws<LexingException>(() => _lexer.Tokenize(input.AsMemory()));

        // Assert
        Assert.That(ex.Message, Does.StartWith("Unclosed string."));
        Assert.That(ex.Index, Is.EqualTo(22));
    }

    [Test]
    [TestCase("\n \r\"broken\ncontinue on new line\"", "broken\ncontinue on new line")]
    [TestCase("\n \r \"broken\rcontinue on new line\"", "broken\rcontinue on new line")]
    [TestCase("\n \r  \"broken\r\ncontinue on new line\"", "broken\r\ncontinue on new line")]
    public void Tokenize_NewLineInString_TokenizesCorrectly(
        string notClosedString,
        string expectedExtractedString)
    {
        // Arrange
        var input = notClosedString;

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());
        var token = (StringToken)tokens.Single();

        // Assert
        Assert.That(token.Kind, Is.EqualTo("TinyLisp"));
        Assert.That(token.Text, Is.EqualTo(expectedExtractedString));
    }

    [Test]
    [TestCase("symbol at end", typeof(LispSymbolToken))]
    [TestCase("keyword at :end", typeof(KeywordToken))]
    //[TestCase("integer at end 1488", typeof(IntegerToken))] // todo
    [TestCase("string at \"end\"", typeof(StringToken))]
    [TestCase("( punctuation at end )", typeof(LispPunctuationToken))]
    [TestCase("comment :somma ;end", typeof(KeywordToken))]
    public void Tokenize_TokenAtEnd_TokenizedCorrectly(string input, Type lastTokenExpectedType)
    {
        // Arrange

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens.Last(), Is.TypeOf(lastTokenExpectedType));
    }

    [Test]
    public void Tokenize_IntAndTheLike_TokenizedCorrectly()
    {
        // Arrange
        var input1 = "1";
        var input2 = "+1";
        var input3 = "-1";
        var input4 = "-133";
        var input5 = "+391";
        var input6 = "+";
        var input7 = "-";
        var input8 = "1-";

        // Act
        var token1 = _lexer.Tokenize(input1.AsMemory()).Single();
        var token2 = _lexer.Tokenize(input2.AsMemory()).Single();
        var token3 = _lexer.Tokenize(input3.AsMemory()).Single();
        var token4 = _lexer.Tokenize(input4.AsMemory()).Single();
        var token5 = _lexer.Tokenize(input5.AsMemory()).Single();
        var token6 = _lexer.Tokenize(input6.AsMemory()).Single();
        var token7 = _lexer.Tokenize(input7.AsMemory()).Single();
        var token8 = _lexer.Tokenize(input8.AsMemory()).Single();

        // Assert
        Assert.That(token1 as Int32Token, Has.Property("Value").EqualTo(1));
        Assert.That(token2 as Int32Token, Has.Property("Value").EqualTo(1));
        Assert.That(token3 as Int32Token, Has.Property("Value").EqualTo(-1));
        Assert.That(token4 as Int32Token, Has.Property("Value").EqualTo(-133));
        Assert.That(token5 as Int32Token, Has.Property("Value").EqualTo(391));
        Assert.That(token6 as LispSymbolToken, Has.Property("SymbolName").EqualTo("+"));
        Assert.That(token7 as LispSymbolToken, Has.Property("SymbolName").EqualTo("-"));
        Assert.That(token8 as LispSymbolToken, Has.Property("SymbolName").EqualTo("1-"));
    }

    [Test]
    [TestCase("a\r")]
    [TestCase("a\r\n")]
    [TestCase("a\n")]
    [TestCase("a\n\r")]
    public void Tokenize_CrAtInputEnd_TokenizedCorrectly(string input)
    {
        // Arrange

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        var token = (LispSymbolToken)tokens.Single();
        Assert.That(token.SymbolName, Is.EqualTo("a"));
    }

    [Test]
    [TestCase(" ;comment\r")]
    [TestCase(" ;comment\r ")]
    [TestCase(" ;comment\n")]
    [TestCase(" ;comment\n ")]
    [TestCase(" ;comment\r\n")]
    [TestCase(" ;comment\n\r")]
    public void Tokenize_CommentWithLineEndings_TokenizedCorrectly(string input)
    {
        // Arrange

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Is.Empty);
    }

    [Test]
    [TestCase("\r\n\"abc\n def\" \"mno \r\n \"  \"lek\r guk\" \"zz\"")]
    public void Tokenize_NewLineInString_PositionIsCorrect(string input)
    {
        // Arrange

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens.Count, Is.EqualTo(4));
        Assert.That(tokens.All(x => x is TextTokenBase), Is.True);

        var stringTokens = tokens.Select(x => (StringToken)x).ToList();

        var stringToken = stringTokens[0];
        Assert.That(stringToken.Kind, Is.EqualTo("TinyLisp"));
        Assert.That(stringToken.Text, Is.EqualTo("abc\n def"));
        Assert.That(stringToken.Position, Is.EqualTo(2));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(10));

        stringToken = stringTokens[1];
        Assert.That(stringToken.Kind, Is.EqualTo("TinyLisp"));
        Assert.That(stringToken.Text, Is.EqualTo("mno \r\n "));
        Assert.That(stringToken.Position, Is.EqualTo(13));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(9));

        stringToken = stringTokens[2];
        Assert.That(stringToken.Kind, Is.EqualTo("TinyLisp"));
        Assert.That(stringToken.Text, Is.EqualTo("lek\r guk"));
        Assert.That(stringToken.Position, Is.EqualTo(24));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(10));

        stringToken = stringTokens[3];
        Assert.That(stringToken.Kind, Is.EqualTo("TinyLisp"));
        Assert.That(stringToken.Text, Is.EqualTo("zz"));
        Assert.That(stringToken.Position, Is.EqualTo(35));
        Assert.That(stringToken.ConsumedLength, Is.EqualTo(4));
    }

    [Test]
    public void Tokenize_BrokenString_TokenizesCorrectly()
    {
        // Arrange
        var input = $"{DQ}line0{CR}line1{CRLF}line2{LF}{DQ}";

        // Act
        var tokens = _lexer.Tokenize(input.AsMemory());

        // Assert
        Assert.That(tokens, Has.Count.EqualTo(1));
        var stringToken = (StringToken)tokens.Single();

        Assert.That(stringToken.Text, Is.EqualTo($"line0{CR}line1{CRLF}line2{LF}"));
        Assert.That(stringToken.Kind, Is.EqualTo("TinyLisp"));
    }

    [Test]
    public void Tokenize_StringEndsWithCr_ThrowsParsingException()
    {
        // Arrange
        var input = $"{DQ}line0{CR}line1{CR}";

        // Act
        var ex = Assert.Throws<LexingException>(() => _lexer.Tokenize(input.AsMemory()));

        // Assert
        Assert.That(ex.Message, Does.StartWith("Unclosed string."));
        Assert.That(ex.Index, Is.EqualTo(13));
    }

    [Test]
    [TestCase("\n(:)", 2)]
    [TestCase("\r\n(:part1:part2)", 3)]
    [TestCase("\r :)", 2)]
    public void Tokenize_SingleColumn_ThrowsParsingException(string input, int expectedIndex)
    {
        // Arrange

        // Act
        var ex = Assert.Throws<TinyLispException>(() => _lexer.Tokenize(input.AsMemory()));

        // Assert
        Assert.That(ex.Message, Does.StartWith("Bad keyword."));
        Assert.That(ex.Index, Is.EqualTo(expectedIndex));
    }

    [Test]
    public void Tokenize_ColonInsideSymbol_ThrowsParsingException()
    {
        // Arrange
        var input = "symbol:bad";

        // Act
        var ex = Assert.Throws<TinyLispException>(() => _lexer.Tokenize(input.AsMemory()));

        // Assert
        Assert.That(ex.Message, Does.StartWith("Bad symbol name."));
        Assert.That(ex.Index, Is.EqualTo(6));
    }

    [Test]
    [TestCase("1111111111111111111111111111111111")]
    [TestCase("+1111111111111111111111111111111111")]
    [TestCase("-1111111111111111111111111111111111")]
    public void Tokenize_SymbolNameCouldBeInteger_ThrowsParsingException(string input)
    {
        // Arrange

        // Act
        var ex = Assert.Throws<TinyLispException>(() => _lexer.Tokenize(input.AsMemory()));

        // Assert
        Assert.That(ex.Message, Does.StartWith("Symbol producer delivered an integer."));
        Assert.That(ex.Index, Is.EqualTo(0));
    }

}