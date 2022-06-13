using TauCode.Parsing.LexicalTokens;
using TauCode.Parsing.ParsingNodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;

namespace TauCode.Parsing.Tests.Parsing.Cli.Nodes;

public class TermNode : ActionNode
{
    public TermNode(
        string term)
        : base(ActionImpl)
    {
        this.Term = term;
    }

    public string Term { get; }

    protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
    {
        if (token is CliWordToken cliWordToken)
        {
            return cliWordToken.Text == this.Term;
        }

        return false;
    }

    private static void ActionImpl(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        var cliResult = (CliParsingResult)parsingResult;
        var thisNode = (TermNode)node;

        cliResult.SetCommand(thisNode.Term);

        parsingResult.IncreaseVersion();
    }
}