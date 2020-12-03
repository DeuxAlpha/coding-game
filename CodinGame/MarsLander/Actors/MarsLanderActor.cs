using System.Collections;
using System.Collections.Generic;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Random;

namespace CodinGame.MarsLander.Actors
{
    public class MarsLanderActor
    {
        public int Score { get; set; }
        public Lander MarsLander { get; }
        private readonly List<string> _actions = new List<string>();
        public IEnumerable<string> Actions => _actions;

        public MarsLanderActor(Lander marsLander)
        {
            MarsLander = Lander.Clone(marsLander);
        }

        public void ActRandomly(int actions)
        {
            for (var i = 0; i < actions; i++)
            {
                var maxNewAngle = MarsLander.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = MarsLander.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = MarsLander.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = MarsLander.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                randomAngle = 0;
                randomPower = 0;

                MarsLander.Apply(randomPower, randomAngle);
                _actions.Add($"{MarsLander.Rotation} {MarsLander.Power}");
            }
        }
    }
}