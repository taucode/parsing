using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tau.Kappa.Parsing;
using Tau.Kappa.Parsing.LexicalTokens;
using Tau.Kappa.Parsing.ParsingNodes;
using Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Result;

namespace Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Nodes;

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