﻿using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Old.TinyLisp;
using TauCode.Parsing.Old.Tokens;
using TauCode.Parsing.Tests.Parsing.Cli.Data;
using TauCode.Parsing.Tests.Parsing.Cli.Data.Entries;

namespace TauCode.Parsing.Old.Tests.Parsing.Cli
{
    [TestFixture]
    public class CliParserTests
    {
        private ILexer _tinyLispLexer;
        private ILexer _cliLexer;

        [SetUp]
        public void SetUp()
        {
            _tinyLispLexer = new OldTinyLispLexer();
            _cliLexer = new OldCliLexer();
        }

        [Test]
        public void CliParser_ValidInput_Parses()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("my-cli");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);
            
            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new OldTinyLispPseudoReader();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var root = builder.Build(nodeFactory, list);

            IOldParser parser = new OldParser();

            
            var commandText =
                "sd --conn \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var cliTokens = _cliLexer.Lexize(commandText);

            var allNodes = root.FetchTree();

            var mm = (ActionNode) allNodes.Single(x =>
                string.Equals(x.Name, "node-serialize-data", StringComparison.InvariantCultureIgnoreCase));
            mm.Action = (node, token, accumulator) =>
            {
                var command = new CliCommand();
                accumulator.AddResult(command);
            };

            // Act
            var cliResults = parser.ParseOld(root, cliTokens);

            // Assert
            var cliCommand = (CliCommand) cliResults.Single();

            var commandEntry = (KeyValueCliCommandEntry)cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "connection", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("Server=.;Database=econera.diet.tracking;Trusted_Connection=True;"));

            commandEntry = (KeyValueCliCommandEntry)cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "provider", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("sqlserver"));

            commandEntry = (KeyValueCliCommandEntry)cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "file", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("c:/temp/mysqlite.json"));
        }

        [Test]
        public void CliParser_TooManyResults_ThrowsUnexpectedTokenException()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("my-cli");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);
            
            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new OldTinyLispPseudoReader();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var root = builder.Build(nodeFactory, list);

            IOldParser parser = new OldParser
            {
                WantsOnlyOneResult = true,
            };
            

            var singleCommand = "sd --conn \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var commandText = $"{singleCommand} {singleCommand}";
            var cliTokens = _cliLexer.Lexize(commandText);

            var allNodes = root.FetchTree();

            var mm = (ActionNode)allNodes.Single(x =>
               string.Equals(x.Name, "node-serialize-data", StringComparison.InvariantCultureIgnoreCase));
            mm.Action = (node, token, accumulator) =>
            {
                var command = new CliCommand();
                accumulator.AddResult(command);
            };

            // Act
            var ex = Assert.Throws<UnexpectedTokenException>(() => parser.ParseOld(root, cliTokens));

            var textToken = (OldTextToken)ex.Token;
            Assert.That(textToken.Text, Is.EqualTo("sd"));
        }
    }
}