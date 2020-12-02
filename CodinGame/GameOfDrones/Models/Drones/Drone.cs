using System.Collections.Generic;

namespace CodinGame.GameOfDrones.Models.Drones
{
    public class Drone
    {
        public Cell Location { get; set; } = new Cell();
        public List<Cell> LocationHistory { get; set; } = new List<Cell>();

        public void UpdateLocation(int x, int y)
        {

            LocationHistory.Add(new Cell
            {
                X = Location.X,
                Y = Location.Y
            });
            Location.X = x;
            Location.Y = y;
        }
    }
}