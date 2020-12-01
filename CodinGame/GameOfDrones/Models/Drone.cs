namespace CodinGame.GameOfDrones.Models
{
    public class Drone
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void UpdateLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}