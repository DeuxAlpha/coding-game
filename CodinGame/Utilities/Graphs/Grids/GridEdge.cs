namespace CodinGame.Utilities.Graphs.Grids
{
    public class GridEdge
    {
        public string OriginId { get; private set; }
        public string DestinationId { get; private set; }
        public GridEdge(string originId, string destinationId)
        {
            OriginId = originId;
            DestinationId = destinationId;
        }
    }
}