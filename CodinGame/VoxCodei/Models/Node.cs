using System;

namespace CodinGame.VoxCodei.Models
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public NodeType NodeType { get; set; }
        public int? Countdown { get; set; }

        public Node(int x, int y, char nodeType)
        {
            X = x;
            Y = y;
            NodeType = nodeType switch
            {
                '@' => NodeType.Active,
                '#' => NodeType.Passive,
                '.' => NodeType.Empty,
                _ => NodeType
            };
        }

        public void MakeBomb()
        {
            if (NodeType == NodeType.Active || NodeType == NodeType.Passive)
                throw new InvalidOperationException("Node is not empty.");
            NodeType = NodeType.Bomb;
            Countdown = 3;
        }
    }
}