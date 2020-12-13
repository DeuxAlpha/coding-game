namespace CodinGame.GreatEscape.v2.Models
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public string Id => $"{X}{Y}";
        public string UpperCellConnectedId { get; private set; }
        public string RightCellConnectedId { get; private set; }
        public string LowerCellConnectedId { get; private set; }
        public string LeftCellConnectedId { get; private set; }
        public string UpperCellId { get; private set; }
        public string RightCellId { get; private set; }
        public string LowerCellId { get; private set; }
        public string LeftCellId { get; private set; }

        private readonly int _maxWidth;
        private readonly int _maxHeight;

        public Cell(int x, int y, int maxWidth, int maxHeight)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
            X = x;
            Y = y;
            if (y - 1 >= 0)
            {
                UpperCellConnectedId = $"{X}{Y - 1}";
                UpperCellId = UpperCellConnectedId;
            }
            if (x + 1 < maxWidth)
            {
                RightCellConnectedId = $"{X + 1}{Y}";
                RightCellId = RightCellConnectedId;
            }
            if (y + 1 < maxHeight)
            {
                LowerCellConnectedId = $"{X}{Y + 1}";
                LowerCellId = LowerCellConnectedId;
            }
            if (x - 1 >= 0)
            {
                LeftCellConnectedId = $"{X - 1}{Y}";
                LeftCellId = LeftCellConnectedId;
            }
        }

        public void RemoveLeftCellConnection()
        {
            LeftCellConnectedId = null;
        }

        public void RemoveUpperCellConnection()
        {
            UpperCellConnectedId = null;
        }

        public void RemoveRightCellConnection()
        {
            RightCellConnectedId = null;
        }

        public void RemoveLowerCellConnection()
        {
            LowerCellConnectedId = null;
        }

        public Cell Clone()
        {
            return new Cell(X, Y, _maxWidth, _maxHeight);
        }
    }
}