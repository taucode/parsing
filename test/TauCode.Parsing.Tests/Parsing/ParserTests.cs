using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Graphs.Building.Impl;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Graphs;
using TauCode.Parsing.Tests.Parsing.Cli.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Parsing.Tests.Parsing;

[TestFixture]
public class ParserTests
{
    private ILexer _cliLexer;
    private IGraph _graph;
    private IParser _parser;

    [SetUp]
    public void SetUp()
    {
        _cliLexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new CliKeyProducer(),
                new CliWordProducer(),
                new JsonStringProducer(),
                new FilePathProducer(),
            },
        };

        var script = this.GetType().Assembly.GetResourceText("cli-grammar-native.lisp", true);
        var reader = new GraphScriptReader();
        var groupMold = reader.ReadScript(script.AsMemory());
        var builder = new GraphBuilder(new CliVertexFactory());
        _graph = builder.Build(groupMold);
        _parser = new Parser
        {
            Root = (IParsingNode)_graph.Single(x => x.Name == "root")
        };
    }

    [Test]
    public void Parse_UnexpectedToken_ThrowsException()
    {
        // Arrange
        // let's corrupt the root node
        var root = (IParsingNode)_graph.Single(x => x.Name == "root");
        var badNode = new PredicateActionNode((node, token, parsingResult) => true, null); // will accept any token
        root.AddLink(badNode);

        var tokens = ReadCliTokens("sd -p sqlserver");
        var result = new CliParsingResult();

        // Act
        var ex = Assert.Throws<ParsingException>(() => _parser.Parse(tokens, result));

        // Assert
        Assert.That(ex.Message, Does.StartWith("Parsing node concurrency occurred."));
        Assert.That(ex.Nodes, Has.Count.EqualTo(2));
        Assert.That(ex.Nodes.ElementAt(0), Is.TypeOf<KeyNode>());
        Assert.That(ex.Nodes.ElementAt(1), Is.EqualTo(badNode));
        Assert.That(ex.Token.ToString(), Is.EqualTo("-p"));
    }

    private IReadOnlyList<ILexicalToken> ReadCliTokens(string clause)
    {
        var tokens = _cliLexer.Tokenize(clause.AsMemory());
        return tokens;
    }
}
