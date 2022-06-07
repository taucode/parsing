using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    // todo regions, clean
    public class GraphBuilder : IGraphBuilder
    {
        private readonly IVertexBuilder _defaultVertexBuilder;
        private readonly IArcBuilder _defaultArcBuilder;

        public GraphBuilder()
        {
            _defaultVertexBuilder = new VertexBuilder();
            _defaultArcBuilder = new ArcBuilder();
        }

        public IEnumerable<IVertexBuilder> CustomVertexBuilders { get; set; }

        public IEnumerable<IArcBuilder> CustomArcBuilders { get; set; }

        public IGraph Build(IGroupMold group)
        {
            var vertexMolds = new List<IVertexMold>();
            var arcMolds = new List<IArcMold>();
            this.WriteVertices(group, vertexMolds, arcMolds);

            var graph = this.CreateGraph();

            var vertexMappings = new Dictionary<IVertexMold, IVertex>();

            foreach (var vertexMold in vertexMolds)
            {
                var vertexBuilder = this.ResolveVertexBuilder(vertexMold);
                var vertex = vertexBuilder.Build(vertexMold);

                graph.Add(vertex);
                vertexMappings.Add(vertexMold, vertex);
            }

            foreach (var arcMold in arcMolds)
            {
                var arcBuilder = this.ResolveArcBuilder(arcMold);
                var arc = arcBuilder.Build(arcMold);

                IVertex tail;
                IVertex head;

                if (arcMold.Tail != null && arcMold.Head != null)
                {
                    tail = vertexMappings[arcMold.Tail];
                    head = vertexMappings[arcMold.Head];
                }
                else if (arcMold.TailPath != null && arcMold.HeadPath != null) 
                {
                    throw new NotImplementedException();
                }
                else if (arcMold.Tail != null && arcMold.HeadPath != null)
                {
                    tail = vertexMappings[arcMold.Tail];
                    var headMold = (IVertexMold)arcMold.Tail.ResolvePath(arcMold.HeadPath); // todo can throw
                    head = vertexMappings[headMold];
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

        protected virtual IArcBuilder ResolveArcBuilder(IArcMold arcMold)
        {
            if (this.CustomArcBuilders != null)
            {
                foreach (var customArcBuilder in this.CustomArcBuilders)
                {
                    if (customArcBuilder.Accepts(arcMold))
                    {
                        return customArcBuilder;
                    }
                }
            }

            return _defaultArcBuilder;
        }

        protected virtual IVertexBuilder ResolveVertexBuilder(IVertexMold vertexMold)
        {
            if (this.CustomVertexBuilders != null)
            {
                foreach (var customVertexBuilder in this.CustomVertexBuilders)
                {
                    if (customVertexBuilder.Accepts(vertexMold))
                    {
                        return customVertexBuilder;
                    }
                }
            }

            return _defaultVertexBuilder;
        }

        protected virtual IGraph CreateGraph()
        {
            return new Graph();
        }

        private void WriteVertices(IGroupMold group, List<IVertexMold> vertexMolds, List<IArcMold> arcMolds)
        {
            foreach (var partMold in group.Content)
            {
                if (partMold is IVertexMold vertexMold)
                {
                    vertexMolds.Add(vertexMold);

                    arcMolds.AddRange(vertexMold.OutgoingArcs);
                    arcMolds.AddRange(vertexMold.IncomingArcs);
                }
                else if (partMold is IGroupMold innerGroupMold)
                {
                    this.WriteVertices(innerGroupMold, vertexMolds, arcMolds);
                }
            }
        }
    }
}
