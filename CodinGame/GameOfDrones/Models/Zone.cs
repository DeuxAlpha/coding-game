namespace CodinGame.GameOfDrones.Models
{
    /// <summary>A zone is a circle with a radius of 100 units.</summary>
    public class Zone
    {
        public Cell Center { get; set; }
        public int OwnerId { get; set; }
    }
}