using QuikGraph;

namespace InsaneScatterbrain.MapGraph
{
    /// <summary>
    /// A graph of connected areas.
    /// </summary>
    public class AreaGraph : UndirectedGraph<Area, AreaGraphEdge>
    {
        public AreaGraph() : base(false) { }
        
        public new AreaGraph Clone()
        {
            var clone = base.Clone();
            var areaGraph = new AreaGraph();
            areaGraph.AddVertexRange(clone.Vertices);
            areaGraph.AddEdgeRange(clone.Edges);
            return areaGraph;
        }
    }
}