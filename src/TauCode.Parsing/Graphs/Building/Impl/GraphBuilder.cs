using System;
using System.Collections.Generic;
using TauCode.Data.Graphs;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    public class GraphBuilder : IGraphBuilder
    {
        #region Fields

        private readonly IVertexFactory _vertexFactory;
        private readonly IArcFactory _arcFactory;

        #endregion

        #region ctor

        public GraphBuilder(IVertexFactory vertexFactory = null, IArcFactory arcFactory = null)
        {
            _vertexFactory = vertexFactory ?? new VertexFactory();
            _arcFactory = arcFactory ?? new ArcFactory();
        }

        #endregion

        #region IGraphBuilder Members

        public IGraph Build(IGroupMold group)
        {
            var groupMolds = new List<IGroupMold>();
            var vertexMolds = new List<IVertexMold>();
            var refMolds = new List<IRefMold>();
            var arcMolds = new List<IArcMold>();
            WriteContent(group, groupMolds, vertexMolds, refMolds, arcMolds);

            var graph = this.CreateGraph();

            //if (refMolds.Any())
            //{
            //    throw new NotImplementedException(); // deal with refs later...
            //}

            var vertexMappings = new Dictionary<IVertexMold, IVertex>();
            //var groups = groupMolds
            //    .Where(x => x.GetFullPath() != null)
            //    .ToDictionary(x => x.GetFullPath(), x => x);

            foreach (var vertexMold in vertexMolds)
            {
                var vertex = _vertexFactory.Create(vertexMold);

                if (vertex == null)
                {
                    throw new BuildingException("Vertex factory returned null as vertex.");
                }

                graph.Add(vertex);
                vertexMappings.Add(vertexMold, vertex);
            }

            foreach (var arcMold in arcMolds)
            {
                var arc = _arcFactory.Create(arcMold);

                IVertex tail;
                IVertex head;

                if (arcMold.Tail != null && arcMold.Head != null)
                {
                    tail = ResolveVertex(arcMold.Tail.GetExitVertexOrThrow(), vertexMappings);
                    head = ResolveVertex(arcMold.Head.GetEntranceVertexOrThrow(), vertexMappings);
                }
                else if (arcMold.TailPath != null && arcMold.HeadPath != null)
                {
                    var tailMold = arcMold.ResolvePath(arcMold.TailPath).GetExitVertexOrThrow();
                    var headMold = arcMold.ResolvePath(arcMold.HeadPath).GetEntranceVertexOrThrow();

                    tail = ResolveVertex(tailMold, vertexMappings);
                    head = ResolveVertex(headMold, vertexMappings);
                }
                else if (arcMold.Tail != null && arcMold.HeadPath != null)
                {
                    tail = ResolveVertex(arcMold.Tail.GetExitVertexOrThrow(), vertexMappings);
                    var headMold = arcMold.Tail.ResolvePath(arcMold.HeadPath);
                    var headEntrance = headMold.GetEntranceVertexOrThrow();
                    head = ResolveVertex(headEntrance, vertexMappings);
                }
                else if (arcMold.TailPath != null && arcMold.Head != null)
                {
                    head = ResolveVertex(arcMold.Head.GetEntranceVertexOrThrow(), vertexMappings);
                    var tailMold = arcMold.Head.ResolvePath(arcMold.TailPath);
                    var tailExit = tailMold.GetExitVertexOrThrow();
                    tail = ResolveVertex(tailExit, vertexMappings);
                }
                else
                {
                    throw new NotImplementedException();
                }

                arc.Connect(tail, head);
            }

            return graph;
        }

        #endregion

        #region Protected

        protected virtual IGraph CreateGraph()
        {
            return new Graph();
        }

        #endregion

        #region Private

        private static IVertex ResolveVertex(
            IVertexMold vertexMold,
            Dictionary<IVertexMold, IVertex> vertexMappings)
        {
            return vertexMappings[vertexMold];
        }

        private static void WriteContent(
            IGroupMold group,
            List<IGroupMold> groupMolds, // todo: need at all?
            List<IVertexMold> vertexMolds,
            List<IRefMold> refMolds,
            List<IArcMold> arcMolds)
        {
            groupMolds.Add(group);

            arcMolds.AddRange(group.OutgoingArcs);
            arcMolds.AddRange(group.IncomingArcs);

            foreach (var scriptElementMold in group.AllElements)
            {
                if (scriptElementMold is IVertexMold vertexMold)
                {
                    vertexMolds.Add(vertexMold);

                    arcMolds.AddRange(vertexMold.OutgoingArcs);
                    arcMolds.AddRange(vertexMold.IncomingArcs);
                }
                else if (scriptElementMold is IRefMold refMold)
                {
                    refMolds.Add(refMold);

                    arcMolds.AddRange(refMold.OutgoingArcs);
                    arcMolds.AddRange(refMold.IncomingArcs);
                }
                else if (scriptElementMold is IGroupMold innerGroupMold)
                {
                    WriteContent(innerGroupMold, groupMolds, vertexMolds, refMolds, arcMolds);
                }
                else if (scriptElementMold is IArcMold arcMold)
                {
                    arcMolds.Add(arcMold);
                }
                else
                {
                    throw new NotImplementedException("error");
                }
            }
        }

        #endregion
    }
}
