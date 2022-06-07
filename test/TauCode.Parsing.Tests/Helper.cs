using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Data.Graphs;

namespace TauCode.Parsing.Tests
{
    internal static class Helper
    {
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

        // todo: add this method to IGraph, along with Contains(), Remove(),
        //internal static void Add(this IGraph graph, IVertex vertex)
        //{
        //    graph.Add(vertex);
        //    graph.UnionWith(new[] { vertex });
        //}
    }
}
