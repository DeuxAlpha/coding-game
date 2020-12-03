using System;
using System.Collections;
using System.Collections.Generic;

namespace CodinGame.GreatEscape.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public Cell Location { get; private set; }
        public List<Cell> LocationHistory { get; } = new List<Cell>();
        public int AvailableWalls { get; set; }
        public Side Origin { get; private set; }
        public Side Destination { get; private set; }
        public string Exit => Destination.ToString("F");

        public void Update(int x, int y, int walls)
        {
            if (Location != null)
                LocationHistory.Add(Location);
            else
                Location = new Cell();
            Location.X = x;
            Location.Y = y;
            AvailableWalls = walls;
        }

        public void Initialize()
        {
            if (Location.X == 0)
            {
                Origin = Side.Left;
                Destination = Side.Right;
            }
            else if (Location.X == GreatEscapeManager.Width - 1)
            {
                Origin = Side.Right;
                Destination = Side.Left;
            }
            else if (Location.Y == 0)
            {
                Origin = Side.Up;
                Destination = Side.Down;
            }
            else if (Location.Y == GreatEscapeManager.Height - 1)
            {
                Origin = Side.Down;
                Destination = Side.Up;
            }
        }
    }
}