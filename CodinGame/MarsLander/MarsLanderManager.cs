using System;
using System.Collections;
using System.Collections.Generic;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Game;

namespace CodinGame.MarsLander
{
    public static class MarsLanderManager
    {
        private static List<SurfaceElement> SurfaceList { get; } = new List<SurfaceElement>();
        public static IEnumerable<SurfaceElement> Surface => SurfaceList;
        public static Lander MarsLander = new Lander();

        public static void Play()
        {
            string[] inputs;
            var surfaceN =
                int.Parse(Console.ReadLine() ?? ""); // the number of points used to draw the surface of Mars.
            for (var i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                var landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
                var
                    landY = int.Parse(
                        inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
                SurfaceList.Add(new SurfaceElement
                {
                    X = landX,
                    Y = landY
                });
            }

            Logger.Log("Surface", SurfaceList);

            // game loop
            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                MarsLander.X = int.Parse(inputs[0]);
                MarsLander.Y = int.Parse(inputs[1]);
                MarsLander.HorizontalSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                MarsLander.VerticalSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                MarsLander.Fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                MarsLander.Rotation = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                MarsLander.Power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
                Console.WriteLine("-20 3");
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}