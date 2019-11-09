﻿using System;
using TauCode.Parsing.Aide.Nodes;
using TauCode.Parsing.Aide.Parsing;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Units;
using TauCode.Parsing.Units.Impl;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide
{
    public class AideParser : ParserBase
    {
        protected override IUnit BuildTree()
        {
            var end = EndNode.Instance;
            var blockDefinitionBlock = this.CreateBlockDefinitionBlock(end);

            var superBlock = new Block()
            {
                Name = "superBlock",
            };

            superBlock.Capture(blockDefinitionBlock, end);
            superBlock.Head = blockDefinitionBlock;

            superBlock.FinalizeUnit();
            return superBlock;
        }

        private IBlock CreateBlockDefinitionBlock(EndNode endNode)
        {
            INode head;
            var blockDefinitionBlock = new Block(head = new SyntaxElementAideNode(
                SyntaxElement.BeginBlock, (token, context) =>
                {
                    var blockParsingResult = new BlockDefinitionResult();
                    context.AddResult(blockParsingResult);
                })
            {
                Name = "Head of BlockDefinition Block",
            })
            {
                Name = "BlockDefinition Block",
            };

            var nameRefsBlock = this.CreateNameReferencesInParenthesesBlock((context, name) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                blockDefinitionResult.Arguments.Add(name);
                context.Modify();
            });
            nameRefsBlock.Name = "NameRefsBlock";

            head.AddLink(nameRefsBlock);

            var contentSplitter = new Splitter();
            nameRefsBlock.GetSingleExitNode().AddLink(contentSplitter);

            // word node
            var wordNode = new WordAideNode((token, context) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                var wordToken = (WordAideToken)token;
                blockDefinitionResult.AddUnitResult(new WordNodeResult(wordToken.Word, wordToken.Name));
                context.Modify();
            })
            {
                Name = "Word",
            };

            // identifier node
            var identifierNode = new SyntaxElementAideNode(
                SyntaxElement.Identifier,
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                    blockDefinitionResult.AddUnitResult(new IdentifierNodeResult(null));
                    context.Modify();
                })
            {
                Name = "Identifier",
            };

            // symbol node
            var symbolNode = new SymbolAideNode((token, context) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                var symbolToken = (SymbolAideToken)token;
                blockDefinitionResult.AddUnitResult(new SymbolNodeResult(symbolToken.Value, null));
                context.Modify();
            })
            {
                Name = "Symbol",
            };

            // block node
            var blockNode = new SyntaxElementAideNode(
                SyntaxElement.Block,
                (token, context) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                var syntaxElementAideToken = (SyntaxElementAideToken)token;
                blockDefinitionResult.AddUnitResult(new BlockResult(syntaxElementAideToken.Name));
                context.Modify();
            })
            {
                Name = "Block"
            };

            // link node
            var linkBlock = this.CreateLinkBlock(out var linkOutNode);

            // adding nodes to content splitter
            contentSplitter.AddWay(wordNode);
            contentSplitter.AddWay(identifierNode);
            contentSplitter.AddWay(symbolNode);
            contentSplitter.AddWay(blockNode);
            contentSplitter.AddWay(linkBlock);

            // beforeEndBlockSplitter
            var beforeEndBlockSplitter = new Splitter();

            wordNode.AddLink(beforeEndBlockSplitter);
            identifierNode.AddLink(beforeEndBlockSplitter);
            symbolNode.AddLink(beforeEndBlockSplitter);
            blockNode.AddLink(beforeEndBlockSplitter);
            linkOutNode.AddLink(beforeEndBlockSplitter);

            // endBlock
            var endBlock = new SyntaxElementAideNode(SyntaxElement.EndBlock, ParsingHelper.IdleTokenProcessor);

            beforeEndBlockSplitter.AddWay(endBlock);
            beforeEndBlockSplitter.AddWay(contentSplitter);

            // adding owned nodes to block
            blockDefinitionBlock.Capture(
                nameRefsBlock,
                contentSplitter,

                wordNode,
                identifierNode,
                symbolNode,
                blockNode,
                linkBlock,

                beforeEndBlockSplitter,
                endBlock);

            endBlock.AddLink(endNode);

            return blockDefinitionBlock;
        }

        private IBlock CreateLinkBlock(out INode outNode)
        {
            var linkNode = new SyntaxElementAideNode(
                SyntaxElement.Link,
                (token, context) =>
                {
                    var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                    var syntaxElementAideToken = (SyntaxElementAideToken)token;
                    blockDefinitionResult.AddUnitResult(new LinkResult(null)); // todo
                    context.Modify();
                });

            var nameRefs = this.CreateNameReferencesInParenthesesBlock((context, s) =>
            {
                var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
                var linkResult = (LinkResult)blockDefinitionResult.GetLastUnitResult<LinkResult>();
                linkResult.AddArgument(s);
                context.Modify();
            });

            linkNode.AddLink(nameRefs);

            var block = new Block(linkNode);

            block.Capture(nameRefs);
            outNode = nameRefs.GetSingleExitNode();
            return block;
        }

        private IBlock CreateNameReferencesInParenthesesBlock(Action<IContext, string> nameAdder)
        {
            var leftParen = new SyntaxElementAideNode(
                SyntaxElement.LeftParenthesis,
                ParsingHelper.IdleTokenProcessor)
            {
                Name = "(",
            };
            var nameRef = new NameReferenceAideNode((token, context) =>
            {
                var nameReferenceToken = (NameReferenceAideToken)token;
                var name = nameReferenceToken.ReferencedName;
                nameAdder(context, name);
            })
            {
                Name = "NameRef",
            };
            var comma = new SyntaxElementAideNode(
                SyntaxElement.Comma,
                ParsingHelper.IdleTokenProcessor)
            {
                Name = ",",
            };
            var rightParen = new SyntaxElementAideNode(
                SyntaxElement.RightParenthesis,
                ParsingHelper.IdleTokenProcessor)
            {
                Name = ")",
            };

            leftParen.AddLink(nameRef);
            nameRef.AddLink(comma);
            nameRef.AddLink(rightParen);
            comma.AddLink(nameRef);

            var block = new Block(leftParen);
            block.Capture(nameRef, comma, rightParen);

            return block;
        }
    }
}
