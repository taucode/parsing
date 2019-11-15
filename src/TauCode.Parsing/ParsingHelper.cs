﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes2;

namespace TauCode.Parsing
{
    public static class ParsingHelper
    {
        public static bool IsEndOfStream(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            return tokenStream.Position == tokenStream.Tokens.Count;
        }

        //public static IToken GetCurrentToken(this ITokenStream tokenStream)
        //{
        //    if (tokenStream.IsEndOfStream())
        //    {
        //        throw new ParserException("Unexpected end of token stream.");
        //    }

        //    var token = tokenStream.Tokens[tokenStream.Position];
        //    return token;
        //}

        public static void AdvanceStreamPosition(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            tokenStream.Position++;
        }

        //public static void IdleTokenProcessor(IToken token, IContext context)
        //{
        //}

        //public static bool IsEndResult(IReadOnlyCollection<IUnit> result)
        //{
        //    if (result == null)
        //    {
        //        throw new ArgumentNullException(nameof(result));
        //    }

        //    var res = result.Count == 1 && result.Single() == EndNode.Instance;
        //    return res;
        //}

        //public static bool IsNestedInto(this IUnit unit, IBlock possibleSuperOwner)
        //{
        //    if (unit == null)
        //    {
        //        throw new ArgumentNullException(nameof(unit));
        //    }

        //    if (possibleSuperOwner == null)
        //    {
        //        throw new ArgumentNullException(nameof(possibleSuperOwner));
        //    }

        //    var currentOwner = unit.Owner;

        //    while (true)
        //    {
        //        if (currentOwner == null)
        //        {
        //            return false;
        //        }

        //        if (currentOwner == possibleSuperOwner)
        //        {
        //            return true;
        //        }

        //        currentOwner = currentOwner.Owner;
        //    }
        //}

        //public static bool IsBlockHeadNode(this INode node)
        //{
        //    var owner = node.Owner;
        //    if (owner == null)
        //    {
        //        return false;
        //    }

        //    return owner.Head == node;
        //}

        //internal static string ToUnitDiagnosticsString(this IUnit unit)
        //{
        //    var diag = $"Unit: type is {unit.GetType().FullName}, name is '{unit.Name}'.";
        //    return diag;
        //}
        //public static void IdleAction(IToken token, IResultAccumulator resultAccumulator)
        //{
        //    // idle
        //}

        public static IReadOnlyCollection<INode2> GetNonIdleNodes(IReadOnlyCollection<INode2> nodes) // todo: optimize. use IEnumerable?
        {
            if (nodes.Any(x => x is IdleNode))
            {
                var list = new List<INode2>();

                foreach (var node in nodes)
                {
                    WriteNonIdleNodes(node, list);
                }

                return list;
            }
            else
            {
                return nodes;
            }
        }

        private static void WriteNonIdleNodes(INode2 node, List<INode2> destination)
        {
            if (node is IdleNode)
            {
                var links = node.Links;
                foreach (var link in links)
                {
                    WriteNonIdleNodes(link, destination);
                }
            }
            else
            {
                destination.Add(node);
            }
        }
    }
}
