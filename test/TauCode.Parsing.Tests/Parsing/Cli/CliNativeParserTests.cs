using System;
using System.Linq;
using NUnit.Framework;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing.LexicalTokenProducers;
using TauCode.Parsing.ParsingNodes;
using TauCode.Parsing.Tests.Parsing.Cli.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    [TestFixture]
    public class CliNativeParserTests
    {
        [Test]
        public void Parse_NativeNodes_ParsesCorrectly()
        {
            // Arrange
            var connectionString = "Server=.;Database=econera.diet.tracking;Trusted_Connection=True;";
            var provider = "sqlserver";
            var filePath = "c:/temp/mysqlite.json";

            var commandText = $"sd --connection \"{connectionString}\" --provider {provider} -f {filePath}";

            var lexer = new Lexer
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

            var root = BuildTree();

            var parser = new Parser
            {
                Root = root,
            };

            // Act
            var tokens = lexer.Tokenize(commandText.AsMemory());
            var result = new CliParsingResult();
            parser.Parse(tokens, result);

            // Assert
            Assert.That(result.Command, Is.EqualTo("sd"));

            Assert.That(result.KeyValues, Has.Count.EqualTo(3));

            Assert.That(result.KeyValues, Does.ContainKey("connection"));
            Assert.That(result.KeyValues["connection"].Single(), Is.EqualTo(connectionString));

            Assert.That(result.KeyValues, Does.ContainKey("provider"));
            Assert.That(result.KeyValues["provider"].Single(), Is.EqualTo(provider));

            Assert.That(result.KeyValues, Does.ContainKey("file"));
            Assert.That(result.KeyValues["file"].Single(), Is.EqualTo(filePath));
        }

        [Test]
        public void BuildTree_NoArgs_BuildsExpectedTree()
        {
            // Arrange
            var nodes = BuildTree().FetchAllVertices();
            var graph = new Graph();

            foreach (var node in nodes)
            {
                graph.Add(node);
            }

            // Act
            var rep = graph.PrintGraph();

            // Assert
            var expectedRep = this.GetType().Assembly.GetResourceText(".expected-sd-graph.txt", true);
            Assert.That(rep, Is.EqualTo(expectedRep));
        }

        private static IParsingNode BuildTree()
        {
            var root = new TermNode("sd")
            {
                Name = "root",
            };

            var idleNode = new IdleNode
            {
                Name = "idle",
            };

            // connection route
            var connectionKeyNode = new KeyNode(new[] { "-c", "--connection" }, "connection", true)
            {
                Name = "connection-key",
            };
            var connectionValueNode = new KeyValueNode("connection")
            {
                Name = "connection-value",
            };

            // provider route
            var providerKeyNode = new KeyNode(new[] { "-p", "--provider" }, "provider", true)
            {
                Name = "provider-key",
            };
            var providerValueNode = new KeyValueNode("provider")
            {
                Name = "provider-value",
            };

            // file route
            var fileKeyNode = new KeyNode(new[] { "-f", "--file" }, "file", true)
            {
                Name = "file-key",
            };
            var fileValueNode = new KeyValueNode("file")
            {
                Name = "file-value",
            };

            var endNode = EndNode.Instance;

            root.AddLink(idleNode);

            idleNode.AddLink(connectionKeyNode);
            idleNode.AddLink(providerKeyNode);
            idleNode.AddLink(fileKeyNode);

            connectionKeyNode.AddLink(connectionValueNode);
            providerKeyNode.AddLink(providerValueNode);
            fileKeyNode.AddLink(fileValueNode);

            connectionValueNode.AddLink(idleNode);
            connectionValueNode.AddLink(endNode);

            providerValueNode.AddLink(idleNode);
            providerValueNode.AddLink(endNode);

            fileValueNode.AddLink(idleNode);
            fileValueNode.AddLink(endNode);

            return root;
        }
    }
}
