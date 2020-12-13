using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Models;

namespace CodinGame.Utilities.Graphs.Grids
{
    /// <summary>This currently only works on a grid I guess...</summary>
    public class GridNodeAStar : GridNode
    {
        /// <summary>Distance from the starting node.</summary>
        public double GCost { get; set; }
        /// <summary>Distance from the end node</summary>
        public double HCost { get; set; }

        /// <summary>GCost + HCost</summary>
        public double FCost => GCost + HCost;

        public GridNodeAStar Parent { get; set; }

        public GridNodeAStar(GridNode thisGridNode, GridNode originGridNode, GridNode targetGridNode)
        {
            X = thisGridNode.X;
            Y = thisGridNode.Y;
            GCost = Trigonometry.GetGridDistance(new Point(X, Y), new Point(originGridNode.X, originGridNode.Y));
            HCost = Trigonometry.GetGridDistance(new Point(X, Y), new Point(targetGridNode.X, targetGridNode.Y));
        }
    }
}