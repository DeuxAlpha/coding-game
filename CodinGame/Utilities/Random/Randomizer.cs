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
    }
}