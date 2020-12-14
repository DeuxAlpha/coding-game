using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.Utilities.Game;

namespace CodinGame.VoxCodei.Models
{
    public class SurveillanceMap
    {
        public Node[,] Nodes { get; }
        private readonly int _maxWidth;
        private readonly int _maxHeight;

        private readonly List<Node> _activeNodes = new List<Node>();
        private readonly List<Node> _passiveNodes = new List<Node>();
        private readonly List<Node> _emptyNodes = new List<Node>();
        private readonly List<Node> _bombNodes = new List<Node>();

        public SurveillanceMap(int width, int height)
        {
            Nodes = new Node[width, height];
            _maxWidth = width;
            _maxHeight = height;
        }

        public void DrawMap()
        {
            for (var rowIndex = 0; rowIndex < _maxHeight; rowIndex++)
            {
                var row = "";
                for (var colIndex = 0; colIndex < _maxWidth; colIndex++)
                {
                    var nodeType = Nodes[colIndex, rowIndex].NodeType;
                    row += nodeType switch
                    {
                        NodeType.Active => "@",
                        NodeType.Passive => "#",
                        _ => "."
                    };
                }

                Logger.Log(row);
            }
        }

        public IEnumerable<Node> GetActiveNodes()
        {
            return _activeNodes.ToList();
        }

        public IEnumerable<Node> GetEmptyNodesAroundNode(Node node)
        {
            return _emptyNodes.Where(emptyNode => emptyNode.X + 1 == node.X && emptyNode.Y == node.Y ||
                                                  emptyNode.X - 1 == node.X && emptyNode.Y == node.Y ||
                                                  emptyNode.Y + 1 == node.Y && emptyNode.X == node.X ||
                                                  emptyNode.Y - 1 == node.Y && emptyNode.X == node.X)
                .ToList();
        }

        public void SetNode(int x, int y, Node node)
        {
            Nodes[x, y] = node;
            switch (node.NodeType)
            {
                case NodeType.Active:
                    _activeNodes.Add(node);
                    break;
                case NodeType.Passive:
                    _passiveNodes.Add(node);
                    break;
                case NodeType.Empty:
                    _emptyNodes.Add(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(node.NodeType));
            }
        }
    }
}