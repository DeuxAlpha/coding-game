using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.GreatEscape.v2.Models;
using CodinGame.GreatEscape.v2.Models.Enums;

namespace CodinGame.GreatEscape.v2
{
    public class GreatEscapeEnvironment
    {
        public Map Map { get; }
        public Map OriginalMap { get; }
        public List<Map> HistoryMaps { get; } = new List<Map>();

        public GreatEscapeEnvironment(int height, int width)
        {
            Map = CreateMap(height, width);
            OriginalMap = new Map(Map.Cells.Select(cell => cell.Clone()).ToList());
        }

        private Map CreateMap(int height, int width)
        {
            var cells = new List<Cell>();
            for (var heightIndex = 0; heightIndex < height; heightIndex++)
            {
                for (var widthIndex = 0; widthIndex < width; widthIndex++)
                {
                    cells.Add(new Cell(heightIndex, widthIndex, width, height));
                }
            }

            return new Map(cells);
        }

        public void UpdateWall(int x, int y, WallDirection wallDirection)
        {
            var originCell = Map.GetCell(x, y);
            if (wallDirection == WallDirection.Horizontal)
            {
                if (originCell.ConnectedUpperCellId != null)
                {
                    var upperCell = Map.GetCell(originCell.ConnectedUpperCellId);
                    upperCell.CutConnectionToBelowCell();
                    originCell.CutConnectionToUpperCell();
                }

                if (originCell.RightCellId != null)
                {
                    var rightCell = Map.GetCell(originCell.RightCellId);
                    var rightAboveCell = Map.GetCell(rightCell.ConnectedUpperCellId);
                    rightCell.CutConnectionToUpperCell();
                    rightAboveCell.CutConnectionToBelowCell();
                }
            }
            else
            {
                if (originCell.ConnectedLeftCellId != null)
                {
                    var leftCell = Map.GetCell(originCell.ConnectedLeftCellId);
                    leftCell.CutConnectionToRightCell();
                    originCell.CutConnectionToLeftCell();
                }
                if (originCell.LowerCellId != null)
                {
                    var lowerCell = Map.GetCell(originCell.LowerCellId);
                    var leftBelowCell = Map.GetCell(lowerCell.ConnectedLeftCellId);
                    lowerCell.CutConnectionToLeftCell();
                    leftBelowCell.CutConnectionToRightCell();
                }
            }
        }

        public void StoreHistory()
        {
            HistoryMaps.Add(new Map(Map.Cells.Select(cell => cell.Clone()).ToList()));
        }
    }
}