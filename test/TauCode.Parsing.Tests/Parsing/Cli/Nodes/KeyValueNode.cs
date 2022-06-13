using System;
using TauCode.Extensions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Nodes;

public class KeyValueNode : ActionNode
{
    public KeyValueNode(string alias)
        : base(ActionImpl)
    {
        this.Alias = alias;
    }

    public string Alias { get; }

    private static void ActionImpl(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var thisNode = (KeyValueNode)node;
        var cliParsingResult = (CliParsingResult)parsingResult;

        cliParsingResult.AddKeyValue(thisNode.Alias, token.ToString());

        parsingResult.IncreaseVersion();
    }

    protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
    {
        return token is TextToken;
    }

    protected override string GetDataTag() => $"Alias: '{this.Alias}'";
}
