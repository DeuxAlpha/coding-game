using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
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

        public void ApplyRandomActions()
        {
            while (Lander.Status == LanderStatus.Flying)
            {
                var maxNewAngle = Lander.Situation.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = Lander.Situation.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = Lander.Situation.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = Lander.Situation.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                Lander.Apply(randomAngle, randomPower, _environment);
            }
        }

        public IEnumerable<string> GetRandomActions(int actions)
        {
            var puppet = Lander.Clone();

            for (var i = 0; i < actions; i++)
            {
                var maxNewAngle = puppet.Situation.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = puppet.Situation.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = puppet.Situation.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = puppet.Situation.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                 puppet.Apply(randomAngle, randomPower, _environment);
            }

            return puppet.Actions;
        }

        public static IEnumerable<string> GetRandomActions(Lander lander, int actions, MarsLanderEnvironment environment)
        {
            var puppet = lander.Clone();

            for (var i = 0; i < actions; i++)
            {
                var maxNewAngle = puppet.Situation.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = puppet.Situation.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = puppet.Situation.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = puppet.Situation.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                puppet.Apply(randomAngle, randomPower, environment);
            }

            return puppet.Actions;
        }

        public IEnumerable<string> GetRandomActions(MarsLanderEnvironment environment)
        {
            var puppet = Lander.Clone();

            while (puppet.Status == LanderStatus.Flying)
            {
                var maxNewAngle = puppet.Situation.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = puppet.Situation.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = puppet.Situation.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = puppet.Situation.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                puppet.Apply(randomAngle, randomPower, environment);
            }

            return puppet.Actions;
        }

        public static IEnumerable<string> GetRandomActions(Lander lander, MarsLanderEnvironment environment)
        {
            var puppet = lander.Clone();

            while (puppet.Status == LanderStatus.Flying)
            {
                var maxNewAngle = puppet.Situation.Rotation + MarsLanderRules.MaxAngleChange;
                var minNewAngle = puppet.Situation.Rotation - MarsLanderRules.MaxAngleChange;
                if (maxNewAngle > MarsLanderRules.MaxAngle) maxNewAngle = MarsLanderRules.MaxAngle;
                if (minNewAngle < MarsLanderRules.MinAngle) minNewAngle = MarsLanderRules.MinAngle;
                var randomAngle = Randomizer.GetValueBetween(minNewAngle, maxNewAngle);

                var maxNewPower = puppet.Situation.Power + MarsLanderRules.MaxPowerChange;
                var minNewPower = puppet.Situation.Power - MarsLanderRules.MaxPowerChange;
                if (maxNewPower > MarsLanderRules.MaxPower) maxNewPower = MarsLanderRules.MaxPower;
                if (minNewPower < MarsLanderRules.MinPower) minNewPower = MarsLanderRules.MinPower;
                var randomPower = Randomizer.GetValueBetween(minNewPower, maxNewPower);

                puppet.Apply(randomAngle, randomPower, environment);
            }

            return puppet.Actions;
        }
    }
}