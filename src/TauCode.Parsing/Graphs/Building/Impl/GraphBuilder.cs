using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    // todo regions, clean
    public class GraphBuilder : IGraphBuilder
    {
        private readonly IVertexFactory _vertexFactory;
        private readonly IArcFactory _arcFactory;

        public GraphBuilder(IVertexFactory vertexFactory = null, IArcFactory arcFactory = null)
        {
            _vertexFactory = vertexFactory ?? new VertexFactory();
            _arcFactory = arcFactory ?? new ArcFactory();
        }

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
                    //tail = vertexMappings[arcMold.Tail];

                    tail = ResolveVertex(arcMold.Tail, vertexMappings, groupRefMolds);
                    head = ResolveVertex(arcMold.Head, vertexMappings, groupRefMolds);
                }
                else if (arcMold.TailPath != null && arcMold.HeadPath != null)
                {
                    throw new NotImplementedException();
                }
                else if (arcMold.Tail != null && arcMold.HeadPath != null)
                {
                    tail = ResolveVertex(arcMold.Tail, vertexMappings, groupRefMolds);
                    var headMold = arcMold.Tail.ResolvePath(arcMold.HeadPath);
                    var headEntrance = headMold.GetEntranceVertex();
                    head = ResolveVertex(headEntrance, vertexMappings, groupRefMolds);
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

        private static IVertex ResolveVertex(
            IVertexMold vertexMold,
            Dictionary<IVertexMold, IVertex> vertexMappings,
            List<IGroupRefMold> groupRefMolds) // todo: groupRefMolds not used.
        {
            if (vertexMold is GroupRefEntranceVertexResolver groupRefEntranceVertexResolver)
            {
                var groupRef = groupRefEntranceVertexResolver.Keeper;
                var groupPath = groupRef.ReferencedGroupPath;

                var referencedGroup = groupRef.Owner.ResolvePath(groupPath);
                var referencedGroupEntrance = referencedGroup.GetEntranceVertex();


                // todo: will get stack overflow in case of cycle in group referencing
                // todo: use ReSharper 'convert recursion to iteration'
                return ResolveVertex(referencedGroupEntrance, vertexMappings, groupRefMolds);
            }
            else if (vertexMold is GroupRefExitVertexResolver groupRefExitVertexResolver)
            {
                var groupRef = groupRefExitVertexResolver.Keeper;
                var groupPath = groupRef.ReferencedGroupPath;

                var referencedGroup = groupRef.Owner.ResolvePath(groupPath);
                var referencedGroupExit = referencedGroup.GetExitVertex();

                
                // todo: will get stack overflow in case of cycle in group referencing
                // todo: use ReSharper 'convert recursion to iteration'
                return ResolveVertex(referencedGroupExit, vertexMappings, groupRefMolds);
            }
            else
            {
                return vertexMappings[vertexMold];
            }
        }

        protected virtual IGraph CreateGraph()
        {
            return new Graph();
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
    }
}
