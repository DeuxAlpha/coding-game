using System;

namespace CodinGame.Utilities.Maths
{
    public static partial class Trigonometry
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

        /// <summary>Calculates the intersection of a line and a horizontal line at an arbitrary y-level.</summary>
        public static double GetXIntersect(double x1, double y1, double x2, double y2, double yLevel)
        {
            var adjustedY1 = y1 - yLevel;
            var adjustedY2 = y2 - yLevel;
            var aboveInterceptTravelPercentage = Math.Abs(adjustedY2) / (Math.Abs(adjustedY2) + Math.Abs(adjustedY1));
            var xIntercept = x1 + (x2 - x1) * aboveInterceptTravelPercentage;
            return xIntercept;
        }

        /// <summary>Calculates the intersection of a line and a vertical line at an arbitrary x-level.</summary>
        public static double GetYIntersect(double x1, double y1, double x2, double y2, double xLevel)
        {
            var adjustedX1 = x1 - xLevel;
            var adjustedX2 = x2 - xLevel;
            var aboveInterceptTravelPercentage = Math.Abs(adjustedX2) / (Math.Abs(adjustedX2) + Math.Abs(adjustedX1));
            var yIntercept = y1 + (y2 - y1) * aboveInterceptTravelPercentage;
            return yIntercept;
        }

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