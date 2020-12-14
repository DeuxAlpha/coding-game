using CodinGame.Utilities.Heaps;

namespace CodinGame.Utilities.Graphs.Grids
{
    public class GridNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Id => $"{X}-{Y}";
        /// <summary>Represents the ID the right neighbour would have; this does not mean the neighbour actually
        /// exists.</summary>
        public string RightNeighbourId => $"{X + 1}-{Y}";
        /// <summary>Represents the ID the left neighbour would have; this does not mean the neighbour actually
        /// exists.</summary>
        public string LeftNeighbourId => $"{X - 1}-{Y}";
        /// <summary>Represents the ID the upper neighbour would have; this does not mean the neighbour actually
        /// exists.</summary>
        public string UpperNeighbourId => $"{X}-{Y - 1}";
        /// <summary>Represents the ID the lower neighbour would have; this does not mean the neighbour actually
        /// exists.</summary>
        public string LowerNeighbourId => $"{X}-{Y + 1}";
    }
}