using System.Collections.Generic;
using System.Linq;

namespace CodinGame.Utilities.Graphs
{
    public class Graph
    {
        public List<Node> Nodes { get; } = new List<Node>();
        public List<Edge> Edges { get; } = new List<Edge>();

        public Graph()
        {

        }

        public void RemoveConnection(string originId, string destinationId, bool bothWays = false)
        {
            var originEdge = Edges
                .FirstOrDefault(edge => edge.OriginId == originId && edge.DestinationId == destinationId);
            if (originEdge != null) Edges.Remove(originEdge);
            if (!bothWays) return;
            var destinationEdge = Edges
                .FirstOrDefault(edge => edge.DestinationId == originId && edge.OriginId == destinationId);
            if (destinationEdge != null) Edges.Remove(destinationEdge);
        }

        public void AddConnection(string originId, string destinationId, double cost = 1)
        {
            var originEdge = Edges
                .FirstOrDefault(edge => edge.OriginId == originId && edge.DestinationId == destinationId);
            if (originEdge != null)
            {
                originEdge.Cost = cost;
                return;
            }

            var newEdge = new Edge(originId, destinationId, cost);
        }
    }
}