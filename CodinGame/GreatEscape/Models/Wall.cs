namespace CodinGame.GreatEscape.Models
{
    public class Wall
    {
        // Start and End Location equal to on which the wall is located at the top left corner of the cell.
        public Cell StartLocation { get; } = new Cell();
        public Cell EndLocation { get; } = new Cell();
    }
}