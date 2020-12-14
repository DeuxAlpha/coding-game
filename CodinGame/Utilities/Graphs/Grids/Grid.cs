using System.Collections.Generic;
using System.Linq;
using CodinGame.Utilities.Extensions;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Heaps;
using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Models;

namespace CodinGame.Utilities.Graphs.Grids
{
    public class Grid
    {
        public List<GridNode> Nodes { get; private set; } = new List<GridNode>();
        public List<GridEdge> Edges { get; private set; } = new List<GridEdge>();

        private int _maxWidth;
        private int _maxHeight;

        public Grid Clone()
        {
            return new Grid
            {
                Edges = Edges.ToList(),
                Nodes = Nodes.ToList()
            };
        }

        private Grid() {}

        public Grid(int width, int height)
        {
            _maxHeight = height;
            _maxWidth = width;
            for (var widthIndex = 0; widthIndex < width; widthIndex++)
            {
                for (var heightIndex = 0; heightIndex < height; heightIndex++)
                {
                    var node = new GridNode
                    {
                        X = widthIndex,
                        Y = heightIndex
                    };
                    Nodes.Add(node);
                    if (widthIndex + 1 < width) Edges.Add(new GridEdge(node.Id, node.RightNeighbourId));
                    if (widthIndex - 1 >= 0) Edges.Add(new GridEdge(node.Id, node.LeftNeighbourId));
                    if (heightIndex + 1 < height) Edges.Add(new GridEdge(node.Id, node.LowerNeighbourId));
                    if (heightIndex - 1 >= 0) Edges.Add(new GridEdge(node.Id, node.UpperNeighbourId));
                }
            }
        }

        public void RemoveConnection(string originId, string destinationId, bool bothWays = true)
        {
            var originEdge = Edges
                .FirstOrDefault(edge => edge.OriginId == originId && edge.DestinationId == destinationId);
            if (originEdge != null)
            {
                Edges.Remove(originEdge);
                Logger.Log($"Cutting Connection between {originId} and {destinationId}");
            }
            if (!bothWays) return;
            var destinationEdge = Edges
                .FirstOrDefault(edge => edge.DestinationId == originId && edge.OriginId == destinationId);
            if (destinationEdge != null)
            {
                Edges.Remove(destinationEdge);
                Logger.Log($"Cutting Connection between {destinationId} and {originId}");
            }
        }

        public void AddConnection(string originId, string destinationId, bool bothWays = true)
        {
            var originEdge = Edges
                .FirstOrDefault(edge => edge.OriginId == originId && edge.DestinationId == destinationId);
            if (originEdge == null)
            {
                var newEdge = new GridEdge(originId, destinationId);
                Edges.Add(newEdge);
            }
            if (!bothWays) return;
            var destinationEdge = Edges
                .FirstOrDefault(edge => edge.DestinationId == originId && edge.OriginId == destinationId);
            if (destinationEdge == null)
            {
                var newEdge = new GridEdge(destinationId, originId);
                Edges.Add(newEdge);
            }
        }

        public bool DoesConnectionExist(string originId, string destinationId)
        {
            return Edges
                .FirstOrDefault(edge => edge.OriginId == originId && edge.DestinationId == destinationId)
                   != null;
        }

        public GridNode GetNode(string nodeId)
        {
            return Nodes.First(node => node.Id == nodeId);
        }

        public IEnumerable<GridNode> GetNeighbours(GridNode gridNode)
        {
            var edgesFromNode = Edges.Where(edge => edge.OriginId == gridNode.Id);
            return edgesFromNode.Select(edge => GetNode(edge.DestinationId));
        }

        /// <summary>Returns the node that need to be followed in order to reach the destination. Currently only works
        /// on a Grid, without costs associated with the movements.</summary>
        // This version with the Heap is actually slower than the one with list.
        public List<GridNodeAStar> GetAStarNodesWithHeap(string originId, string targetId)
        {
            var origin = GetNode(originId);
            var target = GetNode(targetId);

            var openSet = new Heap<GridNodeAStar>(_maxWidth * _maxHeight);
            var closedSet = new Heap<GridNodeAStar>(_maxWidth * _maxHeight);

            var aStarOrigin = new GridNodeAStar(origin, origin, target);

            openSet.Add(aStarOrigin);

            while (openSet.Any())
            {
                var currentNode = openSet.Pop();

                closedSet.Add(currentNode);

                if (currentNode.Id == target.Id)
                {
                    // Found the target node. Retracing steps.
                    var path = new List<GridNodeAStar>();
                    var pathPosition = currentNode;
                    while (pathPosition.Id != origin.Id)
                    {
                        path.Add(pathPosition);
                        pathPosition = pathPosition.Parent;
                    }

                    path.Reverse();
                    return path;
                }

                foreach (var neighbour in GetNeighbours(currentNode).Select(node => new GridNodeAStar(node, origin, target)))
                {
                    if (closedSet.Items.FirstOrDefault(node => node != null && node.Id == neighbour.Id) != null) continue;

                    var newMovementCostToNeighbour = currentNode.GCost + Trigonometry.GetGridDistance(
                        new Point(currentNode.X, currentNode.Y),
                        new Point(neighbour.X, neighbour.Y));

                    var neighbourInOpenSet = openSet.Items.FirstOrDefault(node => node != null && node.Id == neighbour.Id);
                    if (neighbourInOpenSet == null)
                    {
                        // Neighbour has not been analyzed yet, so we are generating the costs and adding to open set.
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = Trigonometry.GetGridDistance(
                            new Point(neighbour.X, neighbour.Y),
                            new Point(target.X, target.Y));
                        neighbour.Parent = currentNode;
                        openSet.Add(neighbour);
                    }
                    if (neighbourInOpenSet != null && newMovementCostToNeighbour > neighbourInOpenSet.GCost)
                    {
                        // Neighbour already exists in open set, but the new movement cost is cheaper, so we're updating it.
                        neighbourInOpenSet.GCost = newMovementCostToNeighbour;
                        neighbourInOpenSet.HCost = Trigonometry.GetDistance(
                            new Point(neighbourInOpenSet.X, neighbourInOpenSet.Y),
                            new Point(target.X, target.Y));
                        neighbourInOpenSet.Parent = currentNode;
                    }
                }
            }

            return new List<GridNodeAStar>();
        }

        /// <summary>Returns the node that need to be followed in order to reach the destination. Currently only works
        /// on a Grid, without costs associated with the movements.</summary>
        // This method is actually faster than the one above, even though it should not be. Might have to looks at some
        // of the algorithm to improve performance.
        public List<GridNodeAStar> GetAStarNodesWithList(string originId, string targetId)
        {
            var origin = GetNode(originId);
            var target = GetNode(targetId);

            var openSet = new List<GridNodeAStar>();
            var closedSet = new HashSet<GridNodeAStar>();

            var aStarOrigin = new GridNodeAStar(origin, origin, target);

            openSet.Add(aStarOrigin);

            while (openSet.Any())
            {
                var currentNode = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.Id == target.Id)
                {
                    // Found the target node. Retracing steps.
                    var path = new List<GridNodeAStar>();
                    var pathPosition = currentNode;
                    while (pathPosition.Id != origin.Id)
                    {
                        path.Add(pathPosition);
                        pathPosition = pathPosition.Parent;
                    }

                    path.Reverse();
                    return path;
                }

                foreach (var neighbour in GetNeighbours(currentNode).Select(node => new GridNodeAStar(node, origin, target)))
                {
                    if (closedSet.FirstOrDefault(node => node.Id == neighbour.Id) != null) continue;

                    var newMovementCostToNeighbour = currentNode.GCost + Trigonometry.GetGridDistance(
                        new Point(currentNode.X, currentNode.Y),
                        new Point(neighbour.X, neighbour.Y));

                    var neighbourInOpenSet = openSet.FirstOrDefault(node => node.Id == neighbour.Id);
                    if (neighbourInOpenSet == null)
                    {
                        // Neighbour has not been analyzed yet, so we are generating the costs and adding to open set.
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = Trigonometry.GetGridDistance(
                            new Point(neighbour.X, neighbour.Y),
                            new Point(target.X, target.Y));
                        neighbour.Parent = currentNode;
                        openSet.Add(neighbour);
                    }
                    if (neighbourInOpenSet != null && newMovementCostToNeighbour > neighbourInOpenSet.GCost)
                    {
                        // Neighbour already exists in open set, but the new movement cost is cheaper, so we're updating it.
                        neighbourInOpenSet.GCost = newMovementCostToNeighbour;
                        neighbourInOpenSet.HCost = Trigonometry.GetDistance(
                            new Point(neighbourInOpenSet.X, neighbourInOpenSet.Y),
                            new Point(target.X, target.Y));
                        neighbourInOpenSet.Parent = currentNode;
                    }
                }
            }

            return new List<GridNodeAStar>();
        }
    }
}