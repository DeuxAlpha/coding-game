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

        // TODO: Find a way to calculate applied force to reach a destination with a specific speed.

        /// <summary>Basically, where on a Y-Axis you would end up if you moved the X distance at a particular angle.</summary>
        public static double GetNewYPosition(double angle, double xLength, int decimals = 3)
        {
            return Math.Round(Math.Tan(ToRadians(angle)) * xLength, decimals);
        }

        /// <summary>Basically, where on a X-Axis you would end up if you moved the Y distance at a particular angle.</summary>
        public static double GetNewXPosition(double angle, double yLength, int decimals = 3)
        {
            return Math.Round(yLength / Math.Tan(ToRadians(angle)), decimals);
        }

        public static double GetHorizontalSpeedFraction(double angle, ZeroDegreesDirection zeroDegreesDirection = ZeroDegreesDirection.Right, int decimals = 3)
        {
            return Math.Round(Math.Cos(ToRadians(angle + GetAdjustedAngle(zeroDegreesDirection))), decimals);
        }

        public static double GetVerticalSpeedFraction(double angle, ZeroDegreesDirection zeroDegreesDirection = ZeroDegreesDirection.Right, int decimals = 3)
        {
            return Math.Round(Math.Sin(ToRadians(angle + GetAdjustedAngle(zeroDegreesDirection))), decimals);
        }

        public static double ToRadians(double angle)
        {
            return Math.PI / 180 * angle;
        }

        public static double ToDegrees(double radians)
        {
            return 180 / Math.PI * radians;
        }

        private static int GetAdjustedAngle(ZeroDegreesDirection zeroDegreesDirection)
        {
            return zeroDegreesDirection switch
            {
                ZeroDegreesDirection.Top => 90,
                ZeroDegreesDirection.Left => 180,
                ZeroDegreesDirection.Bottom => 270,
                ZeroDegreesDirection.Right => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(zeroDegreesDirection), zeroDegreesDirection, null)
            };
        }
    }
}