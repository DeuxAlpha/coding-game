using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.TimeCheat;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Random;

namespace CodinGame.MarsLander.Actors
{
    public class MarsLanderEvolution
    {
        public List<MarsLanderActor> Actors { get; set; }
        public List<Generation> Generations { get; } = new List<Generation>();
        public Lander FinalLander { get; private set; }
        public int? MaxActions { get; set; }

        private readonly MarsLanderEnvironment _environment;
        private readonly AiWeight _aiWeight;
        private readonly RandomNessProvider _randomNessProvider = new RandomNessProvider();

        public MarsLanderEvolution(MarsLanderEnvironment environment, AiWeight aiWeight)
        {
            _environment = environment;
            _aiWeight = aiWeight;
        }

        private void GenerateActors(int count, Lander original)
        {
            Actors = new List<MarsLanderActor>();

            for (var i = 0; i < count; i++)
            {
                Actors.Add(new MarsLanderActor(original, _environment));
            }
        }

        private void CreateBehavior()
        {
            foreach (var actor in Actors)
            {
                if (MaxActions != null)
                {
                    actor.ApplyActions(actor.GetRandomActions((int) MaxActions, _randomNessProvider));
                }
                else
                {
                    actor.ApplyFullRangeRandomActions(_randomNessProvider);
                    // actor.ApplyRandomActions();
                }
            }
        }

        private void Evolve()
        {
            var actorAndScoreList = Actors.Select(actor => ScoreActor(actor, _environment)).OrderBy(actor => actor.Score).ToList();

            Generations.Add(new Generation
            {
                Actors = actorAndScoreList,
                GenerationNumber = Generations.Count + 1
            });

            var betterBias = _aiWeight.BetterBias; // How likely it is that a better actor is used than a worse one.
            var betterCutoff = _aiWeight.BetterCutoff; // Percentage of what are considered good actors vs bad ones.
            var mutationChance = _aiWeight.MutationChance;
            var betterBiasCount = (int) Math.Round(Actors.Count * betterCutoff);

            foreach (var actor in Actors)
            {
                var firstActor = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var secondActor = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var puppet = new MarsLanderActor(firstActor.Lander, _environment);
                var actions = new List<string>();
                var moreActorActionCount = firstActor.Lander.Actions.Count > secondActor.Lander.Actions.Count
                    ? firstActor.Lander.Actions.Count
                    : secondActor.Lander.Actions.Count;
                // TODO: Move this to a method
                if (MaxActions == null)
                {
                    for (var actionIndex = 0; actionIndex < moreActorActionCount; actionIndex++)
                    {
                        var action =
                            Randomizer.Gamble(
                                mutationChance) ? // If mutating, generate new action based on current puppet (to properly get randomized values on puppet state)
                            puppet.GetRandomActions(1, _randomNessProvider).First() :
                            Randomizer
                                .FlipCoin() ? // Otherwise, flip coin and either get action from first or second actor.
                            firstActor.Lander.Actions.ElementAtOrDefault(actionIndex) ?? puppet.GetRandomActions(1, _randomNessProvider).First() :
                            secondActor.Lander.Actions.ElementAtOrDefault(actionIndex) ?? puppet.GetRandomActions(1, _randomNessProvider).First();
                        puppet.ApplyAction(action);
                        actions.Add(action);
                        if (puppet.Lander.WillHitLandingZone(_environment, 1))
                        {
                            puppet.ApplyAction("0 0");
                            actions.Add(action);
                        }

                        // Basically creating an entirely new actor.
                        actor.Reset();
                        actor.ApplyActions(actions); // Acting to get score for next evolution
                        if (actor.Lander.Status == LanderStatus.Flying)
                            actor.ApplyFullRangeRandomActions(_randomNessProvider);
                    }
                }
                else
                {
                    for (var actionIndex = 0; actionIndex < moreActorActionCount && actionIndex < MaxActions; actionIndex++)
                    {
                        var action =
                            Randomizer.Gamble(
                                mutationChance) ? // If mutating, generate new action based on current puppet (to properly get randomized values on puppet state)
                            puppet.GetRandomActions(1, _randomNessProvider).First() :
                            Randomizer
                                .FlipCoin() ? // Otherwise, flip coin and either get action from first or second actor.
                            firstActor.Lander.Actions.ElementAtOrDefault(actionIndex) ?? puppet.GetRandomActions(1, _randomNessProvider).First() :
                            secondActor.Lander.Actions.ElementAtOrDefault(actionIndex) ?? puppet.GetRandomActions(1, _randomNessProvider).First();
                        puppet.ApplyAction(action);
                        actions.Add(action);
                        if (puppet.Lander.WillHitLandingZone(_environment, 1))
                        {
                            puppet.ApplyAction("0 0");
                            actions.Add(action);
                        }

                        actor.Reset();
                        actor.ApplyActions(actions);
                    }
                }
            }
        }

        private GenerationActor GetBiasedActor(
            double betterBias,
            int betterBiasCount,
            IReadOnlyList<GenerationActor> actorAndScoreList)
        {
            return Randomizer.Gamble(betterBias)
                ? actorAndScoreList[Randomizer.GetValueBetween(0, betterBiasCount)]
                : actorAndScoreList[Randomizer.GetValueBetween(betterBiasCount, Actors.Count - 1)];
        }

        private GenerationActor ScoreActor(MarsLanderActor actor, MarsLanderEnvironment environment)
        {
            var horizontalSpeed = actor.Lander.Situation.HorizontalSpeed;
            var verticalSpeed = actor.Lander.Situation.VerticalSpeed;
            var rotation = actor.Lander.Situation.Rotation;
            var distanceFromFlatSurface = environment.GetDistanceFromFlatSurfaceCenter(actor.Lander);
            var distanceFromFlatSurfaceCenter = environment.GetDistanceFromFlatSurfaceCenter(actor.Lander);
            return new GenerationActor
            {
                Lander = actor.Lander.Clone(),
                Score =
                    Math.Abs(horizontalSpeed) * _aiWeight.HorizontalSpeedWeight +
                    Math.Abs(verticalSpeed) * _aiWeight.VerticalSpeedWeight +
                    Math.Abs(rotation) * _aiWeight.RotationWeight +
                    Math.Abs(distanceFromFlatSurfaceCenter.HorizontalDistance) * _aiWeight.HorizontalCenterDistanceWeight
            };
        }

        public void Run(int generations, int population, Lander original)
        {
            GenerateActors(population, original);
            CreateBehavior();

            for (var i = 0; i < generations; i++)
            {
                Evolve();
            }

            var finalActorAndScore = Actors.Select(actor => ScoreActor(actor, _environment)).OrderBy(actor => actor.Score).First();

            FinalLander = finalActorAndScore.Lander;
            Logger.Log("Final Score", finalActorAndScore.Score);
        }
    }
}