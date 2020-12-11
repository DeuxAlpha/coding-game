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
}