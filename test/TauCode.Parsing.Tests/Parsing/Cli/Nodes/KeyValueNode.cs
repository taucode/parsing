using System;
using TauCode.Parsing.LexicalTokens;
using TauCode.Parsing.ParsingNodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;

namespace TauCode.Parsing.Tests.Parsing.Cli.Nodes;

public class KeyValueNode : ActionNode
{
    public KeyValueNode(string alias)
        : base(AcceptsMethod, ActMethod)
    {
        this.Alias = alias;

        this.Name = this.Alias; // todo temp for debugger visualizer
    }

    public string Alias { get; }

    public Func<ActionNode, TextToken, IParsingResult, bool> AdditionalCheck { get; set; }

    private static bool AcceptsMethod(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var thisNode = (KeyValueNode)node;
        if (token is TextToken textToken)
        {
            return thisNode.AdditionalCheck?.Invoke(node, textToken, parsingResult) ?? true;
        }

        return false;
    }

    private static void ActMethod(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var thisNode = (KeyValueNode)node;
        var cliParsingResult = (CliParsingResult)parsingResult;

        cliParsingResult.AddKeyValue(thisNode.Alias, token.ToString());
    }
}