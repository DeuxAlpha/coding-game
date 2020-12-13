namespace CodinGame.Utilities.Graphs
{
    public class Node
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Id => $"{X}{Y}";
    }
}