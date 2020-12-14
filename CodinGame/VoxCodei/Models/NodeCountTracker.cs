using System.Collections.Generic;

namespace CodinGame.VoxCodei.Models
{
    public class NodeCountTracker
    {
        public Node Node { get; set; }
        public int NodeCount { get; set; }

        public NodeCountTracker(Node node)
        {
            Node = node;
            NodeCount = 1;
        }
    }
}