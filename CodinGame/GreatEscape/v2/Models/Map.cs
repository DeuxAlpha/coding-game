using System.Collections.Generic;
using System.Linq;
using CodinGame.GreatEscape.v1.Models;

namespace CodinGame.GreatEscape.v2.Models
{
    public class Map
    {
        public List<Cell> Cells { get; set; }

        public Map(List<Cell> cells)
        {
            Cells = cells;
        }

        public Cell GetCell(string id)
        {
            return Cells.First(cell => cell.Id == id);
        }

        public Cell GetCell(int x, int y)
        {
            return Cells.First(cell => cell.X == x && cell.Y == y);
        }
    }
}