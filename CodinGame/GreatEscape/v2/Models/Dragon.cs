using System.Collections;
using System.Collections.Generic;
using CodinGame.GreatEscape.v2.Models.Enums;

namespace CodinGame.GreatEscape.v2.Models
{
    public class Dragon
    {
        public Cell Location { get; private set; }
        public List<Cell> LocationHistory { get; } = new List<Cell>();
        public int AvailableWalls { get; set; }
        public PlayerType PlayerType { get; set; }

        public Dragon(PlayerType playerType)
        {

        }

        public void Update(Cell location, int availableWalls)
        {
            LocationHistory.Add(Location.Clone());
            Location = location;
            AvailableWalls = availableWalls;
        }
    }
}