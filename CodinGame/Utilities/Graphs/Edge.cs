namespace CodinGame.Utilities.Graphs
{
    public class Edge
    {
        public string OriginId { get; set; }
        public string DestinationId { get; set; }
        public double Cost { get; set; }

        public Edge(string originId, string destinationId, double cost = 1.0)
        {
            OriginId = originId;
            DestinationId = destinationId;
            Cost = cost;
        }
    }
}