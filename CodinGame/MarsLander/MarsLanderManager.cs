using System;
using System.Collections.Generic;
using CodinGame.MarsLander.Models;

namespace CodinGame.MarsLander
{
    public static class MarsLanderManager
    {
        public static List<SurfaceElement> Surface { get; } = new List<SurfaceElement>();

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
                Surface.Add(new SurfaceElement
                {
                    X = landX,
                    Y = landY
                });
            }

            // game loop
            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                var x = int.Parse(inputs[0]);
                var y = int.Parse(inputs[1]);
                var hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                var vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                var fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                var rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                var power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
                Console.WriteLine("-20 3");
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}