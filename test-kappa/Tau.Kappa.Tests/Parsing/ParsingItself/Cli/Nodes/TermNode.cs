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

namespace Tau.Kappa.Tests.Parsing.ParsingItself.Cli.Nodes
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
