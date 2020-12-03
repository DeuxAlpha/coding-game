using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Random;

namespace CodinGame.MarsLander.Actors
{
    public class MarsLanderActor
    {
        public Lander Lander { get; private set; }
        public Lander Original { get; set; }
        private List<string> _actions = new List<string>();
        public IEnumerable<string> Actions => _actions;

        public MarsLanderActor(Lander lander)
        {
            Original = lander.Clone();
            Lander = lander.Clone();
        }

        public void Reset()
        {
            Lander = Original.Clone();
        }

        public void ApplyActions()
        {
            foreach (var actionArray in _actions.Select(action => action.Split(" ")))
            {
                Lander.Apply(int.Parse(actionArray[0]), int.Parse(actionArray[1]));
            }
        }

        public void ApplyAction(string action)
        {
            var actionArray = action.Split(" ");
            Lander.Apply(int.Parse(actionArray[0]), int.Parse(actionArray[1]));
        }

        public void StoreActions(IEnumerable<string> actions)
        {
            _actions = actions.ToList();
        }

        public IEnumerable<string> GetRandomActions(int actions)
        {
            var puppet = Lander.Clone();
            var actionList = new List<string>();

            for (var i = 0; i < actions; i++)
            {
                var maxNewAngle = puppet.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = puppet.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = puppet.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = puppet.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                puppet.Apply(randomAngle, randomPower);
                actionList.Add($"{randomAngle} {randomPower}");
            }

            return actionList;
        }
    }
}