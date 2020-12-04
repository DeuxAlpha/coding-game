using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Random;

namespace CodinGame.MarsLander.Actors
{
    public class MarsLanderEvolution
    {
        public List<MarsLanderActor> Actors { get; set; }
        public List<List<ActorAndScore>> Generations { get; } = new List<List<ActorAndScore>>();
        public MarsLanderActor FinalActor { get; private set; }

        private void GenerateActors(int count, Lander original)
        {
            Actors = new List<MarsLanderActor>();

            for (var i = 0; i < count; i++)
            {
                Actors.Add(new MarsLanderActor(original));
            }
        }

        private void CreateBehavior(int actions)
        {
            foreach (var actor in Actors)
            {
                actor.StoreActions(actor.GetRandomActions(actions));
                actor.ApplyActions();
            }
        }

        private void Evolve()
        {
            var actorAndScoreList = Actors.Select(ScoreActor).OrderBy(actor => actor.Score).ToList();

            Generations.Add(actorAndScoreList);

            const double betterBias = 0.8; // How likely it is that a better actor is used than a worse one.
            const double betterCutoff = 0.2; // Percentage of what are considered good actors vs bad ones.
            const double mutationChance = 0.01;
            var betterBiasCount = (int) Math.Round(Actors.Count * betterCutoff);

            foreach (var actor in Actors)
            {
                var firstActor = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var secondActor = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var puppet = new MarsLanderActor(firstActor.Actor.Original);
                var actions = new List<string>();
                for (var actionIndex = 0; actionIndex < firstActor.Actor.Actions.Count(); actionIndex++)
                {
                    var action =
                        Randomizer.Gamble(
                            mutationChance) ? // If mutating, generate new action based on current puppet (to properly get randomized values on puppet state)
                        puppet.GetRandomActions(1).First() :
                        Randomizer
                            .FlipCoin() ? // Otherwise, flip coin and either get action from first or second actor.
                        firstActor.Actor.Actions.ElementAt(actionIndex) :
                        secondActor.Actor.Actions.ElementAt(actionIndex);
                    puppet.ApplyAction(action);
                    actions.Add(action);
                }

                // Basically creating an entirely new actor.
                actor.Reset();
                actor.StoreActions(actions);
                actor.ApplyActions(); // Acting to get score for next evolution
            }
        }

        private ActorAndScore GetBiasedActor(
            double betterBias,
            int betterBiasCount,
            IReadOnlyList<ActorAndScore> actorAndScoreList)
        {
            return Randomizer.Gamble(betterBias)
                ? actorAndScoreList[Randomizer.GetValueBetween(0, betterBiasCount)]
                : actorAndScoreList[Randomizer.GetValueBetween(betterBiasCount, Actors.Count - 1)];
        }

        private static ActorAndScore ScoreActor(MarsLanderActor actor)
        {
            var distanceFromSurface = MarsLanderManager.GetDistanceFromSurface(actor.Lander);
            var horizontalSpeed = actor.Lander.HorizontalSpeed;
            var verticalSpeed = actor.Lander.VerticalSpeed;
            var rotation = actor.Lander.Rotation;
            var distanceFromFlatSurface = MarsLanderManager.DistanceFromFlatSurfaceCenter(actor.Lander);
            return new ActorAndScore
            {
                Actor = actor,
                Score =
                    Math.Abs(distanceFromSurface) * 1 +
                    Math.Abs(horizontalSpeed) * 3 +
                    Math.Abs(verticalSpeed) * 1 +
                    Math.Abs(rotation) * 0.5 +
                    Math.Abs(distanceFromFlatSurface.VerticalDistance) * 10 +
                    Math.Abs(distanceFromFlatSurface.HorizontalDistance) * 10
            };
        }

        public void Run(int generations, int population, int actions, Lander original)
        {
            GenerateActors(population, original);
            CreateBehavior(actions);

            for (var i = 0; i < generations; i++)
            {
                Evolve();
            }

            var finalActorAndScore = Actors.Select(ScoreActor).OrderBy(actor => actor.Score).First();

            FinalActor = finalActorAndScore.Actor;
            Logger.Log("Final Score", finalActorAndScore.Score);
        }
    }
}