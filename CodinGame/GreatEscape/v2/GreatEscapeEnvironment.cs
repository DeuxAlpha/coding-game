using System;
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

        private static Map CreateMap(int height, int width)
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
                if (originCell.UpperCellConnectedId != null)
                {
                    var upperCell = Map.GetCell(originCell.UpperCellConnectedId);
                    upperCell.RemoveLowerCellConnection();
                    originCell.RemoveUpperCellConnection();
                }

                if (originCell.RightCellId != null)
                {
                    var rightCell = Map.GetCell(originCell.RightCellId);
                    var rightAboveCell = Map.GetCell(rightCell.UpperCellConnectedId);
                    rightCell.RemoveUpperCellConnection();
                    rightAboveCell.RemoveLowerCellConnection();
                }
            }
            else
            {
                if (originCell.LeftCellConnectedId != null)
                {
                    var leftCell = Map.GetCell(originCell.LeftCellConnectedId);
                    leftCell.RemoveRightCellConnection();
                    originCell.RemoveLeftCellConnection();
                }
                if (originCell.LowerCellId != null)
                {
                    var lowerCell = Map.GetCell(originCell.LowerCellId);
                    var leftBelowCell = Map.GetCell(lowerCell.LeftCellConnectedId);
                    lowerCell.RemoveLeftCellConnection();
                    leftBelowCell.RemoveRightCellConnection();
                }
            }
        }

        /// <summary>Returns the actions that need to be taken (LEFT, RIGHT, UP, DOWN) to get to the closest exit.
        /// </summary>
        public IEnumerable<string> GetFastestActionsToExit(Dragon dragon)
        {
            throw new NotImplementedException();
        }

        public void StoreHistory()
        {
            HistoryMaps.Add(new Map(Map.Cells.Select(cell => cell.Clone()).ToList()));
        }

        public IEnumerable<string> GetPossibleActions(Dragon dragon)
        {
            var possibleActions = new List<string>();
            if (dragon.Location.LowerCellConnectedId != null) possibleActions.Add("DOWN");
            if (dragon.Location.RightCellConnectedId != null) possibleActions.Add("RIGHT");
            if (dragon.Location.UpperCellConnectedId != null) possibleActions.Add("UP");
            if (dragon.Location.LeftCellConnectedId != null) possibleActions.Add("LEFT");
            // TODO: Add possible wall actions
            return possibleActions;
        }
    }
}