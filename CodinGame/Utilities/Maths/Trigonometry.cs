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
    }
}