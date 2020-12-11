using System;

namespace CodinGame.Utilities.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsZero(this double theDouble, double consideredZero = 1e-10)
        {
            return Math.Abs(theDouble) < consideredZero;
        }
    }

    public static class Double
    {
        public const double SmallValue = -100_000_000_000;
        public const double BigValue = 100_000_000_000;
    }
}