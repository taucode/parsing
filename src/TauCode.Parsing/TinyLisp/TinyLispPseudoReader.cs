using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.LexicalTokens;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispPseudoReader : ITinyLispPseudoReader
    {
        public PseudoList Read(IList<ILexicalToken> tokens)
        {
            var list = new PseudoList();
            var index = 0;

            this.ReadPseudoListContent(list, tokens, ref index, 0);
            return list;
        }

        private void ReadPseudoListContent(PseudoList list, IList<ILexicalToken> tokens, ref int index, int depth)
        {
            while (true)
            {
                if (index == tokens.Count)
                {
                    if (depth > 0)
                    {
                        throw Helper.CreateException(ParsingErrorTag.TinyLispUnclosedForm, index);
                    }
                    else
                    {
                        return;
                    }
                }

                var token = tokens[index];
                if (token is LispPunctuationToken punctuationToken)
                {
                    switch (punctuationToken.Value)
                    {
                        case Punctuation.RightParenthesis:
                            if (depth == 0)
                            {
                                throw Helper.CreateException(ParsingErrorTag.TinyLispUnexpectedRightParenthesis, index);
                            }
                            else
                            {
                                index++;
                                return;
                            }

                        case Punctuation.LeftParenthesis:
                            index++;
                            var innerList = new PseudoList();
                            this.ReadPseudoListContent(innerList, tokens, ref index, depth + 1);
                            list.AddElement(innerList);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (token is KeywordToken keywordToken)
                {
                    var element = Symbol.Create(keywordToken.Keyword);
                    list.AddElement(element);
                    index++;
                }
                else if (token is LispSymbolToken symbolToken)
                {
                    var element = Symbol.Create(symbolToken.SymbolName);
                    list.AddElement(element);
                    index++;
                }
                else if (token is StringToken stringToken)
                {
                    var element = new StringAtom(stringToken.Text);
                    list.AddElement(element);
                    index++;
                }
                else
                {
                    throw Helper.CreateException(
                        ParsingErrorTag.TinyLispCannotReadToken,
                        index,
                        token.GetType().FullName);
                }
            }
        }
    }
}
