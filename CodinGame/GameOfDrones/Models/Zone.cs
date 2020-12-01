namespace CodinGame.GameOfDrones.Models
{
    /// <summary>A zone is a circle with a radius of 100 units.</summary>
    public class Zone
    {
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int OwnerId { get; set; }
    }
}