using System;

namespace CodinGame.Utilities.Maths
{
    public static class Trigonometry
    {
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            var xDistance = x1 - x2;
            var yDistance = y1 - y2;
            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }

        public static double GetAngle(double x1, double y1, double x2, double y2)
        {
            var xDistance = x1 - x2;
            var yDistance = y1 - y2;
            var angle =  Math.Atan2(yDistance, xDistance) * 180.0 / Math.PI - 180;
            if (angle < 0) return angle + 360;
            if (angle >= 360) return angle - 360;
            return angle;
        }

        public static double GetY(double angle, double xLength, int decimals = 3)
        {
            return Math.Round(Math.Tan(ToRadians(angle)) * xLength, decimals);
        }

        public static double GetX(double angle, double yLength, int decimals = 3)
        {
            return Math.Round(yLength / Math.Tan(ToRadians(angle)), decimals);
        }

        public static double GetHorizontalSpeedFraction(double angle, int decimals = 3)
        {
            return Math.Round(Math.Cos(ToRadians(angle)), decimals);
        }

        public static double GetVerticalSpeedFraction(double angle, int decimals = 3)
        {
            return Math.Round(Math.Sin(ToRadians(angle)), decimals);
        }

        public static double ToRadians(double angle)
        {
            return Math.PI / 180 * angle;
        }

        public static double ToDegrees(double radians)
        {
            return 180 / Math.PI * radians;
        }
    }
}