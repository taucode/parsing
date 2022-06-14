using System.Collections.Generic;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Nodes;

public class KeyNode : ActionNode
{
    public KeyNode(IEnumerable<string> keyValues, string alias, bool isUnique)
        : base(ActionImpl)
    {
        this.KeyValues = new HashSet<string>(keyValues);
        this.Alias = alias;
        this.IsUnique = isUnique;
    }

    public HashSet<string> KeyValues { get; }
    public string Alias { get; }
    public bool IsUnique { get; }

    protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
    {
        if (token is CliKeyToken cliKeyToken)
        {
            return this.KeyValues.Contains(cliKeyToken.Text);
        }

        return false;
    }

    protected override string GetDataTag() => $"Alias: '{this.Alias}'";

    private static void ActionImpl(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var thisNode = (KeyNode)node;
        var cliKeyToken = (CliKeyToken)token;
        var cliParsingResult = (CliParsingResult)parsingResult;

        parsingResult.IncreaseVersion();

        // todo: obey 'IsUnique'

        // do nothing, KeyValueNode will do its job
    }
}
