using System;
using CodinGame.Utilities.Maths.Models;

namespace CodinGame.Utilities.Maths
{
    public static partial class Trigonometry
    {
        public static double GetDistance(Point point1, Point point2)
        {
            var xDistance = point1.X - point2.X;
            var yDistance = point1.Y - point2.Y;
            return Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }

        public static double GetAngle(Point point1, Point point2)
        {
            var xDistance = point1.X - point2.X;
            var yDistance = point1.Y - point2.Y;
            var angle = Math.Atan2(yDistance, xDistance) * 180.0 / Math.PI - 180;
            if (angle < 0) return angle + 360;
            if (angle >= 360) return angle - 360;
            return angle;
        }

        /// <summary>Basically, where on a Y-Axis you would end up if you moved the X distance at a particular angle.</summary>
        public static double GetNewYPosition(double angle, double xLength, int decimals = 3)
        {
            var radians = ToRadians(angle);
            var tangent = Math.Tan(radians);
            var newYPosition = tangent * xLength;
            var rounded = Math.Round(newYPosition, decimals);
            return rounded;
        }

        /// <summary>Basically, where on a X-Axis you would end up if you moved the Y distance at a particular angle.</summary>
        public static double GetNewXPosition(double angle, double yLength, int decimals = 3)
        {
            var radians = ToRadians(angle);
            var tangent = Math.Tan(radians);
            var newXPosition = yLength / tangent;
            var rounded = Math.Round(newXPosition, decimals);
            return rounded;
        }

        public static double GetHorizontalSpeedFraction(
            double angle,
            ZeroDegreesDirection zeroDegreesDirection = ZeroDegreesDirection.Right,
            int decimals = 3)
        {
            return Math.Round(Math.Cos(ToRadians(angle + GetAdjustedAngle(zeroDegreesDirection))), decimals);
        }

        public static double GetVerticalSpeedFraction(
            double angle,
            ZeroDegreesDirection zeroDegreesDirection = ZeroDegreesDirection.Right,
            int decimals = 3)
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

        /// <summary>Returns the distance of two points on a grid. Does not allow for diagonal moves.</summary>
        public static double GetGridDistance(Point origin, Point destination)
        {
            return Math.Abs(origin.X - destination.X) + Math.Abs(origin.Y - destination.Y);
        }
    }
}