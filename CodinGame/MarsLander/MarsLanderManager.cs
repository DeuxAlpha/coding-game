using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Actors;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Maths;

namespace CodinGame.MarsLander
{
    public static class MarsLanderManager
    {
        private static List<string> _winActions = new List<string>();

        private static readonly Lander Lander = new Lander();
        private static MarsLanderEnvironment _environment;

        public static void Play()
        {
            string[] inputs;
            var surfaceN =
                int.Parse(Console.ReadLine() ?? ""); // the number of points used to draw the surface of Mars.
            var surfaceList = new List<SurfaceElement>();
            for (var i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                var landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
                var
                    landY = int.Parse(
                        inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
                surfaceList.Add(new SurfaceElement
                {
                    X = landX,
                    Y = landY
                });
            }

            _environment = new MarsLanderEnvironment(surfaceList);

            var evolution = new MarsLanderEvolution(_environment);

            // game loop
            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                Lander.Situation.X = int.Parse(inputs[0]);
                Lander.Situation.Y = int.Parse(inputs[1]);
                Lander.Situation.HorizontalSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                Lander.Situation.VerticalSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                Lander.Situation.Fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                Lander.Situation.Rotation = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                Lander.Situation.Power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                var potentialWinAction = _environment.GetPotentialWinActions(Lander);
                if (potentialWinAction != null)
                {
                    Actions.Commit(potentialWinAction);
                    continue;
                }

                if (evolution.FinalLander == null || !evolution.FinalLander.Actions.Any())
                    evolution.Run(40, 100, 4, Lander);

                // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
                Actions.Commit(evolution.FinalLander.Actions.First());

                evolution.FinalLander.Actions.RemoveAt(0);
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}