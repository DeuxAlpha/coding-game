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
            var mutationChance = _aiWeight.MutationChance;
            var betterBiasCount = (int) Math.Round(Actors.Count * betterCutoff);

            // TODO: Two steps per actor
            for (var actorIndex = 0; actorIndex < Actors.Count; actorIndex += 2)
            {
                var firstParent = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var secondParent = GetBiasedActor(betterBias, betterBiasCount, actorAndScoreList);
                var moreParentActionCount = firstParent.Lander.Actions.Count > secondParent.Lander.Actions.Count
                    ? firstParent.Lander.Actions.Count
                    : secondParent.Lander.Actions.Count;
                // TODO: Apply new crossover model from https://www.codingame.com/blog/genetic-algorithm-mars-lander/
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
                        var randomModifier = Randomizer.GetValueBetween(0.0, 1.0, 2);
                        string childAction;
                        if (firstParentAction == null && secondParentAction == null || Randomizer.Gamble(mutationChance))
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
                                    var newAngle = Math.Round(randomModifier * firstParentAngle + (1 - randomModifier) * secondParentAngle);
                                    var newPower = Math.Round(randomModifier * firstParentPower + (1 - randomModifier) * secondParentPower);
                                    childAction = $"{newAngle} {newPower}";
                                }
                                else
                                {
                                    var newAngle = Math.Round((1 - randomModifier) * firstParentAngle + randomModifier * secondParentAngle);
                                    var newPower = Math.Round((1 - randomModifier) * firstParentPower + randomModifier * secondParentPower);
                                    childAction = $"{newAngle} {newPower}";
                                }
                            }
                        }

                        childPuppet.ApplyAction(childAction);
                        childActions.Add(childAction);
                        if (!childPuppet.Lander.WillHitLandingZone(_environment, 1)) continue;
                        childPuppet.ApplyAction("0 0");
                        childActions.Add("0 0");
                        break;
                    }
                    // Basically creating an entirely new actor.
                    Actors[actorIndex + childIndex].Reset();
                    Actors[actorIndex + childIndex].ApplyActions(childActions); // Acting to get score for next evolution
                    if (Actors[actorIndex + childIndex].Lander.Status == LanderStatus.Flying && MaxActions == null)
                        Actors[actorIndex + childIndex].ApplyFullRangeRandomActions(_randomNessProvider);
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
            if (actor.Lander.Status == LanderStatus.Landed)
                return new GenerationActor
                {
                    Lander = actor.Lander.Clone(),
                    Score = 0
                };
            var horizontalSpeed = actor.Lander.Situation.HorizontalSpeed;
            var verticalSpeed = actor.Lander.Situation.VerticalSpeed;
            var rotation = actor.Lander.Situation.Rotation;
            var distanceFromFlatSurface = environment.GetDistanceFromFlatSurfaceCenter(actor.Lander);
            var distanceFromFlatSurfaceCenter = environment.GetDistanceFromFlatSurfaceCenter(actor.Lander);
            return new GenerationActor
            {
                Lander = actor.Lander.Clone(),
                Score = Math.Round(
                    Math.Abs(horizontalSpeed) * _aiWeight.HorizontalSpeedWeight +
                    Math.Abs(verticalSpeed) * _aiWeight.VerticalSpeedWeight +
                    Math.Abs(rotation) * _aiWeight.RotationWeight +
                    Math.Abs(distanceFromFlatSurfaceCenter.HorizontalDistance) *
                    _aiWeight.HorizontalCenterDistanceWeight, 2)
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

            var finalActorAndScore = Actors.Select(actor => ScoreActor(actor, _environment))
                .OrderBy(actor => actor.Score).First();

            FinalLander = finalActorAndScore.Lander;
            Logger.Log("Final Score", finalActorAndScore.Score);
        }
    }
}