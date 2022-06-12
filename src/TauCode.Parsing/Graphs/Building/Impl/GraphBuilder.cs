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
            var arcMolds = new List<IArcMold>();
            this.WriteVertices(group, vertexMolds, arcMolds);

            var graph = this.CreateGraph();

            var vertexMappings = new Dictionary<IVertexMold, IVertex>();

            foreach (var vertexMold in vertexMolds)
            {
                var vertex = _vertexFactory.Create(vertexMold);

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
        
        protected virtual IGraph CreateGraph()
        {
            return new Graph();
        }

        private void WriteVertices(IGroupMold group, List<IVertexMold> vertexMolds, List<IArcMold> arcMolds)
        {
            foreach (var partMold in group.AllElements)
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
