using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.Utilities.Random;

namespace CodinGame.MarsLander.TimeCheat
{
    /// <summary>Class that holds a bunch of random angles in memory in order to limit the amount of times Randomizer
    /// needs to be called during processing time.</summary>
    public class RandomNessProvider
    {

        public Stack<int> RandomAngles { get; private set; }
        public Stack<int> RandomPower { get; private set; }

        private readonly int _cacheSize;

        public RandomNessProvider(int cacheSize = 100_000)
        {
            _cacheSize = cacheSize;
            ReStack();
        }

        private void ReStack()
        {
            var randomAngles = new Stack<int>();
            var randomPower = new Stack<int>();
            for (var i = 0; i < _cacheSize; i++)
            {
                if (randomAngles.Count < _cacheSize)
                    randomAngles.Push(Randomizer.GetValueBetween(MarsLanderRules.MinAngle, MarsLanderRules.MaxAngle));
                if (randomPower.Count < _cacheSize)
                    randomPower.Push(Randomizer.GetValueBetween(MarsLanderRules.MinPower, MarsLanderRules.MaxPower));
                if (randomPower.Count == _cacheSize && randomAngles.Count == _cacheSize)
                    break;
            }

            RandomAngles = randomAngles;
            RandomPower = randomPower;
        }

        public int GetRandomAngle()
        {
            var randomAngle = RandomAngles.Pop();
            if (RandomAngles.Count <= 0) ReStack();
            return randomAngle;
        }

        public int GetRandomPower()
        {
            var randomPower = RandomPower.Pop();
            if (RandomPower.Count <= 0) ReStack();
            return randomPower;
        }
    }
}