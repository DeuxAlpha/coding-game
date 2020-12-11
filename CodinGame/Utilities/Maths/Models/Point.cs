namespace CodinGame.Utilities.Maths.Models
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(){}

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point point1, Point point2)
        {
            return point1.Add(point2);
        }

        public Point Add(Point point)
        {
            return new Point(X + point.X, Y + point.Y);
        }

        public static Point operator -(Point point1, Point point2)
        {
            return point1.Subtract(point2);
        }

        public Point Subtract(Point point)
        {
            return new Point(X - point.X, Y - point.Y);
        }

        public static double operator *(Point point1, Point point2)
        {
            return point1.Dot(point2);
        }

        public double Dot(Point point)
        {
            return X * point.X + Y * point.Y;
        }

        public static double operator /(Point point1, Point point2)
        {
            return point1.Cross(point2);
        }

        public double Cross(Point point)
        {
            return X * point.Y - Y * point.X;
        }

        public static Point operator *(Point point, double multiplier)
        {
            return point.Multiply(multiplier);
        }

        public static Point operator *(double multiplier, Point point)
        {
            return point.Multiply(multiplier);
        }

        public Point Multiply(double multiplier)
        {
            return new Point(X * multiplier, Y * multiplier);
        }
    }
}