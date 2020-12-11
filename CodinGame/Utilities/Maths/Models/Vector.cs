namespace CodinGame.Utilities.Maths.Models
{
    public class Vector
    {
        public Point Point1 { get; }
        public Point Point2 { get; }
        private double? _angle;
        public double Angle => GetAngle();
        private double? _crossProduct;
        public double CrossProduct => GetCrossProduct();
        private double? _dotProduct;
        public double DotProduct => GetDotProduct();

        public Vector(double x1, double y1, double x2, double y2)
        {
            Point1 = new Point(x1, y1);
            Point2 = new Point(x2, y2);
        }

        public Vector(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
        private double GetDotProduct()
        {
            if (_dotProduct != null) return (double) _dotProduct;
            _dotProduct = Point1.X * Point2.Y + Point2.X * Point2.Y;
            return (double) _dotProduct;
        }

        private double GetCrossProduct()
        {
            if (_crossProduct != null) return (double) _crossProduct;
            _crossProduct = Point1.X * Point2.Y - Point2.X * Point1.Y;
            return (double) _crossProduct;
        }

        private double GetAngle()
        {
            if (_angle != null) return (double) _angle;
            _angle = Trigonometry.GetAngle(Point1, Point2);
            return (double) _angle;
        }
    }
}