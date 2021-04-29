using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tau.Kappa.Parsing;
using Tau.Kappa.Parsing.LexicalTokenProducers;
using Tau.Kappa.Parsing.ParsingNodes;
using Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Nodes;
using Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Result;

namespace Tau.Kappa.Tests.Parsing.ParsingItself.Cli
{
    [TestFixture]
    public class CliNativeParserTests
    {
        [Test]
        public void Parse_ValidInput_ParsesCorrectly()
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

            var root = new TermNode("sd");

            var idleNode = new IdleNode();

            var connectionKeyNode = new KeyNode(new[] { "-c", "--connection" }, "connection", true);
            var connectionValueNode = new KeyValueNode("connection");

            var providerKeyNode = new KeyNode(new[] { "-p", "--provider" }, "provider", true);
            var providerValueNode = new KeyValueNode("provider");

            var fileKeyNode = new KeyNode(new[] { "-f", "--file" }, "file", true);
            var fileValueNode = new KeyValueNode("file");

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
    }
}
