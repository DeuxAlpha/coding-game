using System;
using System.Collections.Generic;
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
            // int? leftX = null;
            // int? leftY = null;
            for (var i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                // X coordinate of a surface point. (0 to 6999)
                var landX = int.Parse(inputs[0]);
                // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
                var landY = int.Parse(inputs[1]);
                // if (leftX == null)
                // {
                // leftX = landX;
                // leftY = landY;
                // continue;
                // }

                // var rightX = landX;
                // var rightY = landY;
                // var surface = new SurfaceZone((int) leftX, (int) leftY, rightX, rightY);
                // surfaceList.Add(surface);
                // leftX = rightX;
                // leftY = rightY;
            }

            var queue = GetQueue();

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

                var action = queue.Dequeue();
                Logger.Log(action);
                Actions.Commit($"{action[0]} {action[1]}");
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static Queue<int[]> GetQueue()
        {
            return new Queue<int[]>(new[]
            {
                new[] {0, 0}, new[] {0, 1}, new[] {-3, 2}, new[] {-6, 2}, new[] {-10, 1}, new[] {-12, 1},
                new[] {-15, 1}, new[] {-11, 2}, new[] {-16, 2}, new[] {-20, 2}, new[] {-27, 3}, new[] {-25, 3},
                new[] {-29, 3}, new[] {-28, 3}, new[] {-33, 4}, new[] {-30, 4}, new[] {-32, 4}, new[] {-31, 4},
                new[] {-28, 4}, new[] {-27, 4}, new[] {-18, 4}, new[] {-16, 4}, new[] {-11, 4}, new[] {-11, 4},
                new[] {-11, 4}, new[] {-11, 4}, new[] {-9, 4}, new[] {-7, 4}, new[] {-10, 4}, new[] {-13, 4},
                new[] {-13, 4}, new[] {-28, 4}, new[] {-25, 4}, new[] {-23, 4}, new[] {-25, 4}, new[] {-24, 4},
                new[] {-26, 4}, new[] {-20, 4}, new[] {-16, 4}, new[] {-11, 4}, new[] {-11, 4}, new[] {-9, 4},
                new[] {-21, 4}, new[] {-12, 4}, new[] {-6, 4}, new[] {-4, 4}, new[] {-3, 4}, new[] {-2, 4},
                new[] {2, 4}, new[] {10, 4}, new[] {3, 4}, new[] {2, 4}, new[] {9, 4}, new[] {15, 4}, new[] {22, 4},
                new[] {28, 4}, new[] {38, 4}, new[] {48, 4}, new[] {56, 4}, new[] {54, 4}, new[] {51, 4}, new[] {42, 4},
                new[] {49, 4}, new[] {48, 4}, new[] {57, 4}, new[] {53, 4}, new[] {52, 4}, new[] {42, 4}, new[] {32, 3},
                new[] {18, 3}, new[] {12, 4}, new[] {10, 4}, new[] {11, 4}, new[] {-1, 4}, new[] {-13, 4},
                new[] {-14, 4}, new[] {-20, 4}, new[] {-12, 4}, new[] {-15, 4}, new[] {-23, 3}, new[] {-23, 4},
                new[] {-29, 4}, new[] {-28, 4}, new[] {-17, 4}, new[] {-29, 3}, new[] {-26, 4}, new[] {-14, 4},
                new[] {-22, 4}, new[] {-10, 4}, new[] {0, 4}
            });
        }
    }
}