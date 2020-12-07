using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.TimeCheat;
using CodinGame.Utilities.Random;

namespace CodinGame.MarsLander.Actors
{
    public class MarsLanderActor
    {
        public Lander Lander { get; private set; }
        private Lander Original { get; }
        private readonly MarsLanderEnvironment _environment;

        public MarsLanderActor(Lander lander, MarsLanderEnvironment environment)
        {
            Original = lander.Clone();
            Lander = lander.Clone();
            _environment = environment;
        }

        public void Reset()
        {
            Lander = Original.Clone();
        }

        public void ApplyActions(IEnumerable<string> actions)
        {
            foreach (var actionArray in actions.Select(action => action.Split(" ")))
            {
                if (Lander.Status != LanderStatus.Flying) continue;
                Lander.Apply(int.Parse(actionArray[0]), int.Parse(actionArray[1]), _environment);
            }
        }

        public void ApplyAction(string action)
        {
            var actionArray = action.Split(" ");
            Lander.Apply(int.Parse(actionArray[0]), int.Parse(actionArray[1]), _environment);
        }

        public void ApplyFullRangeRandomActions(RandomNessProvider randomNessProvider)
        {
            while (Lander.Status == LanderStatus.Flying)
            {
                var randomAngle = randomNessProvider.GetRandomAngle();
                var randomPower = randomNessProvider.GetRandomPower();

                Lander.Apply(randomAngle, randomPower, _environment);
            }
        }

        public IEnumerable<string> GetRandomActions(int actions, RandomNessProvider randomNessProvider)
        {
            var puppet = Lander.Clone();

            for (var i = 0; i < actions; i++)
            {
                var randomAngle = randomNessProvider.GetRandomAngle();
                var randomPower = randomNessProvider.GetRandomPower();
                 puppet.Apply(randomAngle, randomPower, _environment);
            }

            return puppet.Actions;
        }


        public IEnumerable<string> GetRandomActions(RandomNessProvider randomNessProvider)
        {
            var puppet = Lander.Clone();

            while (puppet.Status == LanderStatus.Flying)
            {
                var randomAngle = randomNessProvider.GetRandomAngle();
                var randomPower = randomNessProvider.GetRandomPower();

                puppet.Apply(randomAngle, randomPower, _environment);
            }

            return puppet.Actions;
        }
    }
}