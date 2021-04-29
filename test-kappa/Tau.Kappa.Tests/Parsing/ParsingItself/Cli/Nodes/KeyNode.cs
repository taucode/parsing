using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tau.Kappa.Parsing;
using Tau.Kappa.Parsing.LexicalTokens;
using Tau.Kappa.Parsing.ParsingNodes;
using Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Result;

namespace Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Nodes;

public class KeyNode : ActionNode
{
    public KeyNode(IEnumerable<string> keyValues, string alias, bool isUnique)
        : base(AcceptsMethod, ActMethod)
    {
        this.KeyValues = new HashSet<string>(keyValues);
        this.Alias = alias;
        this.IsUnique = isUnique;

        this.Name = this.Alias; // todo temp for debugger visualizer
    }

    public HashSet<string> KeyValues { get; }
    public string Alias { get; }
    public bool IsUnique { get; }

    private static bool AcceptsMethod(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var thisNode = (KeyNode)node;
        if (token is CliKeyToken cliKeyToken)
        {
            return thisNode.KeyValues.Contains(cliKeyToken.Text);
        }

        return false;
    }

    private static void ActMethod(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var thisNode = (KeyNode)node;
        var cliKeyToken = (CliKeyToken)token;
        var cliParsingResult = (CliParsingResult)parsingResult;

        // todo: obey 'IsUnique'

        // do nothing, KeyValueNode will do its job
    }
}
