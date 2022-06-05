using TauCode.Parsing.LexicalTokens;
using TauCode.Parsing.ParsingNodes;
using TauCode.Parsing.Tests.Parsing.Cli.Result;

namespace TauCode.Parsing.Tests.Parsing.Cli.Nodes
{
    public class TermNode : ActionNode
    {
        public TermNode(
            string term)
            : base(AcceptsMethod, ActMethod)
        {
            this.Term = term;
            this.Name = this.Term; // todo temp?
        }

        public string Term { get; }

        private static bool AcceptsMethod(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
        {
            var thisNode = (TermNode)node;

            if (token is CliWordToken cliWordToken)
            {
                return cliWordToken.Text == thisNode.Term;
            }

            return false;
        }

        private static void ActMethod(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
        {
            var cliResult = (CliParsingResult)parsingResult;
            var thisNode = (TermNode)node;

            cliResult.SetCommand(thisNode.Term);
        }
    }
}
