using System;

namespace CodinGame.Utilities.Random
{
    public static class Randomizer
    {
        private static readonly System.Random Rand = new System.Random();

        /// <summary>Inclusively returns a random value between the two provided values.</summary>
        public static int GetValueBetween(int minValue, int maxValue)
        {
            return Rand.Next(minValue, maxValue + 1);
        }

        public static double GetValueBetween(double minimum, double maximum, int? decimals = null)
        {
            var randomValue = Rand.NextDouble() * (maximum - minimum) + minimum;
            return decimals != null ? Math.Round(randomValue, (int) decimals) : randomValue;
        }

        public static bool Gamble(double chance)
        {
            return GetValueBetween(0.0, 1.0) < chance;
        }

        public static bool FlipCoin()
        {
            return GetValueBetween(0.0, 1.0) < 0.5;
        }
    }
}