namespace CodinGame.GreatEscape.v2.Models
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public string Id => $"{X}{Y}";
        public string ConnectedUpperCellId { get; private set; }
        public string ConnectedRightCellId { get; private set; }
        public string ConnectedLowerCellId { get; private set; }
        public string ConnectedLeftCellId { get; private set; }
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
                ConnectedUpperCellId = $"{X}{Y - 1}";
                UpperCellId = ConnectedUpperCellId;
            }
            if (x + 1 < maxWidth)
            {
                ConnectedRightCellId = $"{X + 1}{Y}";
                RightCellId = ConnectedRightCellId;
            }
            if (y + 1 < maxHeight)
            {
                ConnectedLowerCellId = $"{X}{Y + 1}";
                LowerCellId = ConnectedLowerCellId;
            }
            if (x - 1 >= 0)
            {
                ConnectedLeftCellId = $"{X - 1}{Y}";
                LeftCellId = ConnectedLeftCellId;
            }
        }

        public void CutConnectionToLeftCell()
        {
            ConnectedLeftCellId = null;
        }

        public void CutConnectionToUpperCell()
        {
            ConnectedUpperCellId = null;
        }

        public void CutConnectionToRightCell()
        {
            ConnectedRightCellId = null;
        }

        public void CutConnectionToBelowCell()
        {
            ConnectedLowerCellId = null;
        }

        public Cell Clone()
        {
            return new Cell(X, Y, _maxWidth, _maxHeight);
        }
    }
}