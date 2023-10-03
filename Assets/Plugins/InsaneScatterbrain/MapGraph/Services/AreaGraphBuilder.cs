using System;
using System.Collections.Generic;
using DelaunatorSharp;
using InsaneScatterbrain.DelaunatorSharp;

namespace InsaneScatterbrain.MapGraph
{
    /// <summary>
    /// Builder to create connected area graphs of lists of areas.
    /// </summary>
    public class AreaGraphBuilder
    {
        private readonly DelaunatorNoAlloc delaunator = new DelaunatorNoAlloc();
        private readonly List<IEdge> edges = new List<IEdge>();

        private readonly Func<AreaGraphEdge> newEdge;

        [Obsolete("Will probably be removed version 2.0. Please use the other constructor.")]
        public AreaGraphBuilder()
        {
            newEdge = () => new AreaGraphEdge();
        }

        public AreaGraphBuilder(Func<AreaGraphEdge> newEdge)
        {
            this.newEdge = newEdge;
        }

        /// <summary>
        /// Builds a connected area graph from a list of areas.
        /// </summary>
        /// <param name="areas">The areas.</param>
        /// <param name="graph">The graph.</param>
        /// <returns>The connected area graph.</returns>
        public void BuildGraph(IList<Area> areas, AreaGraph graph)
        {
            graph.Clear();
            if (areas.Count == 0) return;
            
            foreach (var area in areas)
            {
                graph.AddVertex(area);
            }
            
            switch (areas.Count)
            {
                case 1:
                    return;
                case 2:
                {
                    var edge01 = newEdge();
                    edge01.Set(areas[0], areas[1]);
                    
                    graph.AddEdge(edge01);
                    return;
                }
                case 3:
                {
                    var edge01 = newEdge();
                    edge01.Set(areas[0], areas[1]);
                    
                    var edge12 = newEdge();
                    edge12.Set(areas[1], areas[2]);
                    
                    var edge20 = newEdge();
                    edge20.Set(areas[2], areas[0]);
                    
                    graph.AddEdge(edge01);
                    graph.AddEdge(edge12);
                    graph.AddEdge(edge20);
                    return;
                }
            }
            
            delaunator.Triangulate(areas);
            delaunator.GetEdges(edges);
            foreach (var edge in edges)
            {
                var areaGraphEdge = newEdge();
                areaGraphEdge.Set(edge.P as Area, edge.Q as Area);
                graph.AddEdge(areaGraphEdge);
            }
        }
    }
}