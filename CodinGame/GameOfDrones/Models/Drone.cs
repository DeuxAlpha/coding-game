using System.Collections.Generic;

namespace CodinGame.GameOfDrones.Models
{
    public class Drone
    {
        public Cell Location { get; set; }
        public List<Cell> LocationHistory { get; set; }

        public void UpdateLocation(int x, int y)
        {
            Location.X = x;
            Location.Y = y;
        }
    }
}