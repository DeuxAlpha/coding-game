using System;

namespace CodinGame.Utilities.Maths
{
    public static class Trigonometry
    {
        public static double GetDistance(int x1, int y1, int x2, int y2)
        {
            var xDistance = x1 - x2;
            var yDistance = y1 - y2;
            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }

        public static double GetAngle(int x1, int y1, int x2, int y2)
        {
            var xDistance = x1 - x2;
            var yDistance = y1 - y2;
            var angle =  Math.Atan2(yDistance, xDistance) * 180.0 / Math.PI - 180;
            if (angle < 0) return angle + 360;
            if (angle >= 360) return angle - 360;
            return angle;
        }

        public static double GetHorizontalSpeedFraction(int angle, int decimals = 3)
        {
            return Math.Round(Math.Cos(ToRadians(angle)), decimals);
        }

        public static double GetVerticalSpeedFraction(int angle, int decimals = 3)
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