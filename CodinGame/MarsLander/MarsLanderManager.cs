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
        private static List<SurfaceElement> SurfaceList { get; } = new List<SurfaceElement>();
        private static List<string> _winActions = new List<string>();

        public static IEnumerable<SurfaceElement> Surface => SurfaceList;
        public static Lander Lander = new Lander();

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

            var evolution = new MarsLanderEvolution();

            // game loop
            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                Lander.X = int.Parse(inputs[0]);
                Lander.Y = int.Parse(inputs[1]);
                Lander.HorizontalSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                Lander.VerticalSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                Lander.Fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                Lander.Rotation = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                Lander.Power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                var potentialWinAction = GetPotentialWinActions();
                if (potentialWinAction != null)
                {
                    Actions.Commit(potentialWinAction);
                    continue;
                }

                if (evolution.FinalActor == null || !evolution.FinalActor.Actions.Any())
                    evolution.Run(40, 100, 4, Lander);

                // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
                Actions.Commit(evolution.FinalActor.Actions.First());

                evolution.FinalActor.Actions.RemoveAt(0);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        public static int GetDistanceFromSurface(Lander lander)
        {
            var leftZoneUnderLander = SurfaceList.First(element => element.X >= lander.X || element.X <= lander.X);
            var leftZoneUnderLanderIndex = SurfaceList.IndexOf(leftZoneUnderLander);
            var rightZoneUnderLander = SurfaceList[leftZoneUnderLanderIndex + 1];
            var zoneAngle = Trigonometry
                .GetAngle(leftZoneUnderLander.X, leftZoneUnderLander.Y, rightZoneUnderLander.X, rightZoneUnderLander.Y);
            return (int) Math.Round(
                Trigonometry.GetY(zoneAngle, lander.X - leftZoneUnderLander.X) +
                leftZoneUnderLander.Y - lander.Y);
        }

        public static Distance DistanceFromFlatSurfaceCenter(Lander lander)
        {
            for (var i = 0; i < SurfaceList.Count; i++)
            {
                var leftSurface = SurfaceList[i];
                var rightSurface = SurfaceList[i + 1];
                if (leftSurface.Y != rightSurface.Y) continue;
                var centerX = rightSurface.X - leftSurface.X;

                return new Distance
                {
                    HorizontalDistance = lander.X - centerX,
                    VerticalDistance = lander.Y - lander.Y,
                    FullDistance = Trigonometry.GetDistance(centerX, leftSurface.Y, lander.X, lander.Y)
                };
            }

            throw new ArgumentOutOfRangeException(nameof(SurfaceList), "I guess there is no flat surface.");
        }

        public static Distance DistanceFromFlatSurface(Lander lander)
        {
            for (var i = 0; i < SurfaceList.Count; i++)
            {
                var leftSurface = SurfaceList[i];
                var rightSurface = SurfaceList[i + 1];
                if (leftSurface.Y != rightSurface.Y) continue;
                var side = Side.Above;
                if (lander.X < leftSurface.X) side = Side.Left;
                if (lander.X > rightSurface.X) side = Side.Right;
                var horizontalDistance = 0;
                var fullDistance = 0.0;
                if (side == Side.Left)
                {
                    horizontalDistance = leftSurface.X - lander.X;
                    fullDistance = Trigonometry.GetDistance(leftSurface.X, leftSurface.Y, lander.X, lander.Y);
                }

                if (side == Side.Right)
                {
                    horizontalDistance = lander.X - rightSurface.X;
                    fullDistance = Trigonometry.GetDistance(rightSurface.X, rightSurface.Y, lander.X, lander.Y);
                }

                if (side == Side.Above)
                {
                    fullDistance = Trigonometry.GetDistance(rightSurface.X, rightSurface.Y, lander.X, lander.Y);
                }

                return new Distance
                {
                    HorizontalDistance = horizontalDistance,
                    VerticalDistance = lander.Y - leftSurface.Y,
                    FullDistance = fullDistance
                };
            }

            throw new ArgumentOutOfRangeException(nameof(SurfaceList), "I guess there is no flat surface.");
        }

        private static string GetPotentialWinActions()
        {
            var puppet = Lander.Clone();
            puppet.LimitMomentum();
            var distanceFromFlatSurface = DistanceFromFlatSurface(puppet);
            if (Math.Abs(distanceFromFlatSurface.HorizontalDistance) > 0)
                return null;
            if (Math.Abs(distanceFromFlatSurface.VerticalDistance) < 50)
                return "0 4";
            return puppet.LimitMomentum();
        }
    }
}