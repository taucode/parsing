using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Data.Graphs;
using TauCode.Extensions;

namespace TauCode.Parsing.Tests
{
    internal static class Helper
    {
        private static readonly HashSet<char> StandardPunctuationChars;

        static Helper()
        {
            var punctList = new List<char>();
            punctList.AddRange(new[]
            {
                '~',
                '?',
                '!',
                '@',
                '#',
                '$',
                '%',
                '^',
                '&',
                '=',
                '*',
                '|',
                '/',
                '+',
                '-',
                '[',
                ']',
                '(',
                ')',
                '{',
                '}',
                '\\',
                '.',
                ',',
                '"',
                '\'',
                ':',
                ';',
                '`',
            });
            StandardPunctuationChars = new HashSet<char>(punctList);


        }

        internal static string PrintGraph(this IGraph graph)
        {
            var sb = new StringBuilder();

            sb.AppendLine("--- Vertices ---");

            var nodeNames = graph
                .Select(x => x.Name ?? "<null_name>")
                .OrderBy(x => x)
                .ToList();

            foreach (var nodeName in nodeNames)
            {
                sb.AppendLine(nodeName);
            }

            sb.AppendLine("--- Arcs ---");
            var arcReps = graph
                .GetArcs()
                .Select(x => x.GetArcRepresentation())
                .OrderBy(x => x)
                .ToList();

            foreach (var arcRep in arcReps)
            {
                sb.AppendLine(arcRep);
            }

            return sb.ToString();
        }

        internal static string GetArcRepresentation(this IArc arc)
        {
            string tailName;
            string headName;

            if (arc.Tail == null)
            {
                tailName = "<null_vertex>";
            }
            else
            {
                tailName = arc.Tail.Name ?? "<null_name>";
            }

            if (arc.Head == null)
            {
                headName = "<null_vertex>";
            }
            else
            {
                headName = arc.Head.Name ?? "<null_name>";
            }

            return $"{tailName} -> {headName}";
        }

        internal static List<IVertex> FetchAllVertices(this IVertex vertex)
        {
            var vertices = new HashSet<IVertex>();

            FetchAllVerticesPriv(vertex, vertices);

            return vertices.ToList();
        }

        private static void FetchAllVerticesPriv(IVertex vertex, HashSet<IVertex> vertices)
        {
            if (vertices.Contains(vertex))
            {
                return;
            }

            vertices.Add(vertex);

            foreach (var linkTo in vertex.OutgoingArcs.Select(x => x.Head))
            {
                FetchAllVerticesPriv(linkTo, vertices);
            }

            foreach (var linkFrom in vertex.IncomingArcs.Select(x => x.Tail))
            {
                FetchAllVerticesPriv(linkFrom, vertices);
            }
        }

        internal static bool IsLatinLetterInternal(this char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return true;
            }

            if (c >= 'A' && c <= 'Z')
            {
                return true;
            }

            return false;
        }

        internal static bool IsDecimalDigit(this char c)
        {
            return c >= '0' && c <= '9';
        }

        internal static bool IsInlineWhiteSpaceOrCaretControl(this char c) => IsInlineWhiteSpace(c) || IsCaretControl(c);

        internal static bool IsStandardPunctuationChar(this char c) => StandardPunctuationChars.Contains(c);

        internal static bool IsInlineWhiteSpace(this char c) => c.IsIn(' ', '\t');

        internal static bool IsCaretControl(this char c) => c.IsIn('\r', '\n');
    }
}
