using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;

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
            var vertexMolds = new List<IVertexMold>();
            var groupRefMolds = new List<IGroupRefMold>();
            var arcMolds = new List<IArcMold>();
            this.WriteContent(group, vertexMolds, groupRefMolds, arcMolds);

            var graph = this.CreateGraph();

            var vertexMappings = new Dictionary<IVertexMold, IVertex>();

            foreach (var vertexMold in vertexMolds)
            {
                var vertex = _vertexFactory.Create(vertexMold);

                if (vertex == null)
                {
                    throw new NotImplementedException(); // WTF???
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
                    tail = ResolveVertex(arcMold.Tail, vertexMappings);
                    head = ResolveVertex(arcMold.Head, vertexMappings);
                }
                else if (arcMold.TailPath != null && arcMold.HeadPath != null)
                {
                    throw new NotImplementedException();
                }
                else if (arcMold.Tail != null && arcMold.HeadPath != null)
                {
                    tail = ResolveVertex(arcMold.Tail, vertexMappings);
                    var headMold = arcMold.Tail.ResolvePath(arcMold.HeadPath);
                    var headEntrance = headMold.GetEntranceVertex();
                    head = ResolveVertex(headEntrance, vertexMappings);
                }
                else if (arcMold.TailPath != null && arcMold.Head != null)
                {
                    throw new NotImplementedException();
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
            if (vertexMold is GroupRefEntranceVertexResolver groupRefEntranceVertexResolver)
            {
                var groupRef = groupRefEntranceVertexResolver.Keeper;
                var groupPath = groupRef.ReferencedGroupPath;

                var referencedGroup = groupRef.Owner.ResolvePath(groupPath);
                var referencedGroupEntrance = referencedGroup.GetEntranceVertex();

                // todo: will get stack overflow in case of cycle in group referencing
                // todo: use ReSharper 'convert recursion to iteration'
                return ResolveVertex(referencedGroupEntrance, vertexMappings);
            }
            else if (vertexMold is GroupRefExitVertexResolver groupRefExitVertexResolver)
            {
                var groupRef = groupRefExitVertexResolver.Keeper;
                var groupPath = groupRef.ReferencedGroupPath;

                var referencedGroup = groupRef.Owner.ResolvePath(groupPath);
                var referencedGroupExit = referencedGroup.GetExitVertex();


                // todo: will get stack overflow in case of cycle in group referencing
                // todo: use ReSharper 'convert recursion to iteration'
                return ResolveVertex(referencedGroupExit, vertexMappings);
            }
            else
            {
                return vertexMappings[vertexMold];
            }
        }

        private void WriteContent(
            IGroupMold group,
            List<IVertexMold> vertexMolds,
            List<IGroupRefMold> groupRefMolds,
            List<IArcMold> arcMolds)
        {
            foreach (var scriptElementMold in group.AllElements)
            {
                if (scriptElementMold is IVertexMold vertexMold)
                {
                    vertexMolds.Add(vertexMold);

                    arcMolds.AddRange(vertexMold.OutgoingArcs);
                    arcMolds.AddRange(vertexMold.IncomingArcs);
                }
                else if (scriptElementMold is IGroupRefMold groupRefMold)
                {
                    groupRefMolds.Add(groupRefMold);

                    arcMolds.AddRange(groupRefMold.GetEntranceVertex().OutgoingArcs);
                    arcMolds.AddRange(groupRefMold.GetEntranceVertex().IncomingArcs);

                    arcMolds.AddRange(groupRefMold.GetExitVertex().OutgoingArcs);
                    arcMolds.AddRange(groupRefMold.GetExitVertex().IncomingArcs);
                }
                else if (scriptElementMold is IGroupMold innerGroupMold)
                {
                    this.WriteContent(innerGroupMold, vertexMolds, groupRefMolds, arcMolds);
                }
            }
        }

        #endregion
    }
}
