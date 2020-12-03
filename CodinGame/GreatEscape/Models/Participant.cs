namespace CodinGame.GreatEscape.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public Cell Location { get; } = new Cell();
        public int AvailableWalls { get; set; }
        public Origin Origin { get; set; }

        public void Update(int x, int y, int walls)
        {
            Location.X = x;
            Location.Y = y;
            AvailableWalls = walls;
        }

        public void SetOrigin()
        {
            if (Location.X == 0)
                Origin = Origin.Left;
            else if (Location.X == GreatEscapeManager.Width - 1)
                Origin = Origin.Right;
            else if (Location.Y == 0)
                Origin = Origin.Top;
            else if (Location.Y == GreatEscapeManager.Height - 1)
                Origin = Origin.Bottom;
        }
    }
}