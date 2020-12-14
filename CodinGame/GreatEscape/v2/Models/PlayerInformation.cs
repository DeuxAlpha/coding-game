using System.Collections.Generic;
using CodinGame.Utilities.Graphs.Grids;

namespace CodinGame.GreatEscape.v2.Models
{
    public class PlayerInformation
    {
        public Dragon Dragon { get; set; }
        public List<GridNodeAStar> Nodes { get; set; }
    }
}