namespace CodinGame.Utilities.Graphs.Grids
{
    public class GridEdge
    {
        public string OriginId { get; set; }
        public string DestinationId { get; set; }

        public GridEdge(string originId, string destinationId)
        {
            OriginId = originId;
            DestinationId = destinationId;
        }
    }
}