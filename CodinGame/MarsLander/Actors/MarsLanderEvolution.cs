using System;
using System.Collections;
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
                    actor.ApplyActions(MarsLanderActor.GetRandomActions((int) MaxActions, _randomNessProvider));
                }
                else
                {
                    actor.ApplyFullRangeRandomActions(_randomNessProvider);
                }
            }
        }

        private void Evolve()
        {
            var actorAndScoreList = Actors.Select(actor => ScoreActor(actor, _environment))
                .OrderBy(actor => actor.Score).ToList();

            Generations.Add(new Generation
            {
                Actors = actorAndScoreList,
                GenerationNumber = Generations.Count + 1
            });


            var betterBias = _aiWeight.BetterBias; // How likely it is that a better actor is used than a worse one.
            var betterCutoff = _aiWeight.BetterCutoff; // Percentage of what are considered good actors vs bad ones.
            var betterBiasCount = (int) Math.Round(Actors.Count * betterCutoff);
            var elitism = _aiWeight.ElitismBias; // The percentage that gets copied directly to the new generation.
            var mutationChance = _aiWeight.MutationChance;
            var elitismCount = (int) Math.Round(Actors.Count * elitism);

            for (var eliteIndex = 0; eliteIndex < elitismCount; eliteIndex++)
            {
                UpdateActor(eliteIndex, actorAndScoreList[eliteIndex].Lander.Actions);
            }

            for (var actorIndex = 0 + elitismCount; actorIndex < Actors.Count; actorIndex += 2)
            {
                var firstParent = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var secondParent = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var randomModifier = Randomizer.GetValueBetween(0.0, 1.0, 2);
                var moreParentActionCount = firstParent.Lander.Actions.Count > secondParent.Lander.Actions.Count
                    ? firstParent.Lander.Actions.Count
                    : secondParent.Lander.Actions.Count;
                for (var childIndex = 0; childIndex < 2; childIndex++)
                {
                    var childPuppet = new MarsLanderActor(firstParent.Lander, _environment);
                    var childActions = new List<string>();
                    for (var actionIndex = 0;
                        actionIndex < moreParentActionCount && actionIndex < (MaxActions ?? int.MaxValue);
                        actionIndex++)
                    {
                        var firstParentAction = firstParent.Lander.Actions.ElementAtOrDefault(actionIndex);
                        var secondParentAction = secondParent.Lander.Actions.ElementAtOrDefault(actionIndex);
                        string childAction;
                        if (firstParentAction == null && secondParentAction == null ||
                            Randomizer.Gamble(mutationChance))
                        {
                            childAction = MarsLanderActor.GetRandomActions(1, _randomNessProvider).First();
                        }
                        else
                        {
                            // If a parent's action index is null, take both from the other parent. The below algorithm is then not needed
                            // --> Because taking both values from the same parent simply results into the same action
                            // Also, depending on the child index, we apply one of two possible algorithms:
                            // ChildIndex = 0: NewAction = randomModifier * firstParentAction + (1 - randomModifier) * secondParentAction
                            // ChildIndex = 1: NewAction = (1 - randomModifier) * firstParentAction + randomModifier * secondParentAction
                            if (firstParentAction == null)
                            {
                                childAction = secondParentAction;
                            }

                            else if (secondParentAction == null)
                            {
                                childAction = firstParentAction;
                            }
                            else
                            {
                                var firstParentActionArray = firstParentAction!.Split(" ");
                                var firstParentAngle = int.Parse(firstParentActionArray[0]);
                                var firstParentPower = int.Parse(firstParentActionArray[1]);
                                var secondParentActionArray = secondParentAction!.Split(" ");
                                var secondParentAngle = int.Parse(secondParentActionArray[0]);
                                var secondParentPower = int.Parse(secondParentActionArray[1]);
                                if (childIndex == 0)
                                {
                                    var newAngle = Math.Round(randomModifier * firstParentAngle +
                                                              (1 - randomModifier) * secondParentAngle);
                                    var newPower = Math.Round(randomModifier * firstParentPower +
                                                              (1 - randomModifier) * secondParentPower);
                                    childAction = $"{newAngle} {newPower}";
                                }
                                else
                                {
                                    var newAngle = Math.Round((1 - randomModifier) * firstParentAngle +
                                                              randomModifier * secondParentAngle);
                                    var newPower = Math.Round((1 - randomModifier) * firstParentPower +
                                                              randomModifier * secondParentPower);
                                    childAction = $"{newAngle} {newPower}";
                                }
                            }
                        }

                        childPuppet.ApplyAction(childAction);
                        childActions.Add(childAction);
                        // TODO: There's something fishy going on here.
                        // It seems like the last action to land is broken.
                        // Landers are stopping over the actual Y coordinates.
                        // And the last action does not get applied (e.g. 0 1 below)
                        // Also, some landers think the landed successfully even though their rotation is wrong.
                        if (!childPuppet.Lander.WillHitLandingZone(_environment, 1)) continue;
                        childPuppet.ApplyAction("0 1");
                        childActions.Add("0 1");
                        break;
                    }

                    UpdateActor(actorIndex + childIndex, childActions);
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

        private void UpdateActor(int index, IEnumerable<string> actions)
        {
            // Basically creating an entirely new actor.
            Actors[index].Reset();
            Actors[index].ApplyActions(actions); // Acting to get score for next evolution
            if (Actors[index].Lander.Status == LanderStatus.Flying && MaxActions == null)
                Actors[index].ApplyFullRangeRandomActions(_randomNessProvider);
        }

        private GenerationActor ScoreActor(MarsLanderActor actor, MarsLanderEnvironment environment)
        {
            if (actor.Lander.Status == LanderStatus.Landed)
                return new GenerationActor
                {
                    Lander = actor.Lander.Clone(),
                    Score = 0
                };
            var distanceFromFlatSurface = environment.GetDistanceFromFlatSurface(actor.Lander);
            var horizontalDistance = Math.Abs(distanceFromFlatSurface.HorizontalDistance);
            var verticalDistance = Math.Abs(distanceFromFlatSurface.VerticalDistance);
            var horizontalSpeed = Math.Abs(actor.Lander.Situation.HorizontalSpeed);
            var verticalSpeed = Math.Abs(actor.Lander.Situation.VerticalSpeed);
            var rotation = Math.Abs(actor.Lander.Situation.Rotation);
            var fuel = actor.Lander.Situation.Fuel;
            // TODO: Need to figure out when to apply which scoring method.
            // if (distanceFromFlatSurface.HorizontalDistance > 0)
            // {
                // We want to first and foremost get to the landing zone. We can worry about speed and rotation after.
                // For that reason, we are going to return a very low score if we didn't even make it to the landing zone.
                // return new GenerationActor
                // {
                    // Lander = actor.Lander.Clone(),
                    // Score = distanceFromFlatSurface.HorizontalDistance * _aiWeight.HorizontalDistanceWeight +
                            // horizontalSpeed * _aiWeight.HorizontalSpeedWeight +
                            // verticalSpeed * _aiWeight.VerticalSpeedWeight +
                            // rotation * _aiWeight.RotationWeight
                // };
            // }

            // return new GenerationActor
            // {
                // Lander = actor.Lander.Clone(),
                // Score =
                    // horizontalSpeed * _aiWeight.HorizontalSpeedWeight +
                    // verticalSpeed * _aiWeight.VerticalSpeedWeight +
                    // rotation * _aiWeight.RotationWeight
            // };
            return new GenerationActor
            {
                Lander = actor.Lander.Clone(),
                Score =
                    horizontalDistance * _aiWeight.HorizontalDistanceWeight +
                    verticalDistance * _aiWeight.VerticalDistanceWeight +
                    horizontalSpeed * _aiWeight.HorizontalSpeedWeight +
                    verticalSpeed * _aiWeight.VerticalSpeedWeight +
                    rotation * _aiWeight.RotationWeight +
                    fuel * _aiWeight.FuelWeight
            };
        }

        public void Run(int generations, int population, Lander original)
        {
            GenerateActors(population, original);
            CreateBehavior();

            for (var i = 0; i < generations; i++)
            {
                Evolve();
                if (Generations.Last().Actors.Any(actor => actor.Lander.Status == LanderStatus.Landed))
                    break;
            }

            var finalActorAndScore = Generations
                .SelectMany(generation => generation.Actors)
                .Select(actor => actor)
                .OrderBy(actor => actor.Score)
                .First();

            FinalLander = finalActorAndScore.Lander;
        }
    }
}