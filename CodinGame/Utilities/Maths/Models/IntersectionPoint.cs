using CodinGame.Utilities.Maths.Enums;

namespace CodinGame.Utilities.Maths.Models
{
    public class IntersectionPoint
    {
        public double? X { get; }
        public double? Y { get; }
        public IntersectionType IntersectionType { get; }

        public IntersectionPoint(IntersectionType intersectionType)
        {
            IntersectionType = intersectionType;
        }

        public IntersectionPoint(double x, double y, IntersectionType intersectionType)
        {
            X = x;
            Y = y;
            IntersectionType = intersectionType;
        }
    }
}