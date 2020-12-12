using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Actors;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Game;

namespace CodinGame.MarsLander
{
    public static class MarsLanderManager
    {
        public static void Play()
        {
            string[] inputs;
            // the number of points used to draw the surface of Mars.
            var surfaceN = int.Parse(Console.ReadLine() ?? "");
            int? leftX = null;
            int? leftY = null;
            var surfaceZones = new List<SurfaceZone>();
            for (var i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                // X coordinate of a surface point. (0 to 6999)
                var landX = int.Parse(inputs[0]);
                // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
                var landY = int.Parse(inputs[1]);
                if (leftX == null)
                {
                    leftX = landX;
                    leftY = landY;
                    continue;
                }

                var rightX = landX;
                var rightY = landY;
                var surface = new SurfaceZone((int) leftX, (int) leftY, rightX, rightY);
                surfaceZones.Add(surface);
                leftX = rightX;
                leftY = rightY;
            }

            var environment = new MarsLanderEnvironment(surfaceZones);
            var weights = new AiWeight
            {
                HorizontalDistanceWeight = 1,
                VerticalDistanceWeight = 1,
                HorizontalSpeedWeight = 1,
                VerticalSpeedWeight = 1,
                RotationWeight = .4,
                FuelWeight = .1,
                BetterBias = .9,
                BetterCutoff = .15,
                MutationChance = .025,
                ElitismBias = .3
            };
            var evolution = new MarsLanderEvolution(environment, weights);
            GenerationActor bestOverallActor = null;
            var situationIndex = 0;

            // game loop
            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                var x = int.Parse(inputs[0]);
                var y = int.Parse(inputs[1]);
                var horizontalSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                var verticalSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                var fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                var rotation = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                var power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                var lander = new Lander
                {
                    Situation = new Situation
                    {
                        X = x,
                        Y = y,
                        HorizontalSpeed = horizontalSpeed,
                        VerticalSpeed = verticalSpeed,
                        Rotation = rotation,
                        Power = power,
                        Fuel = fuel
                    }
                };

                // TODO: Implement the algorithm.
                // This is how it's going to work: Every tick, we are creating 10 generations with 10 populations. We
                // are applying the very first action of the best actor of all the generations and keeping its score.
                // Next, we do the same once more. If there is an actor with a better score, we apply his first action.
                // Otherwise, we stick with the actor from the previous evolution. As we proceed downward, we will
                // continue to have better and better actors for every evolution. What's useful about this method is that
                // if we have an actor with score 0, it will always continue to be picked without the chance of it getting
                // overridden.

                evolution.Run(10, 10, lander);

                var bestEvolutionActor = evolution.Generations
                    .SelectMany(generation => generation.Actors)
                    .OrderBy(actor => actor.Score)
                    .First();

                if (bestOverallActor == null || bestEvolutionActor.Score < bestOverallActor.Score)
                {
                    situationIndex = 0;
                    bestOverallActor = bestEvolutionActor;
                }

                Logger.Log("Best Score", bestOverallActor.Score);
                var situation = bestOverallActor.Lander.Situations.ElementAtOrDefault(situationIndex);
                Logger.Log($"Applying Situation from index '{situationIndex}'", situation);

                if (situation == null)
                {
                    Actions.Commit("0 4");
                }
                else
                {
                    Actions.Commit($"{situation.Rotation} {situation.Power}");
                }


                situationIndex += 1;
                evolution.ClearGenerations();
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}