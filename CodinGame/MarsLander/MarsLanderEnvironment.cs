using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Maths;

namespace CodinGame.MarsLander
{
    public class MarsLanderEnvironment
    {
        private List<SurfaceElement> _surfaceList;

        public MarsLanderEnvironment(List<SurfaceElement> surfaceList)
        {
            _surfaceList = surfaceList;
        }

        public int GetDistanceFromSurface(Lander lander)
        {
            var leftZoneUnderLander = _surfaceList.First(element => element.X >= lander.Situation.X || element.X <= lander.Situation.X);
            var leftZoneUnderLanderIndex = _surfaceList.IndexOf(leftZoneUnderLander);
            var rightZoneUnderLander = _surfaceList[leftZoneUnderLanderIndex + 1];
            var zoneAngle = Trigonometry
                .GetAngle(leftZoneUnderLander.X, leftZoneUnderLander.Y, rightZoneUnderLander.X, rightZoneUnderLander.Y);
            return (int) Math.Round(
                Trigonometry.GetY(zoneAngle, lander.Situation.X - leftZoneUnderLander.X) +
                leftZoneUnderLander.Y - lander.Situation.Y);
        }

        public Distance DistanceFromFlatSurfaceCenter(Lander lander)
        {
            for (var i = 0; i < _surfaceList.Count; i++)
            {
                var leftSurface = _surfaceList[i];
                var rightSurface = _surfaceList[i + 1];
                if (leftSurface.Y != rightSurface.Y) continue;
                var centerX = rightSurface.X - leftSurface.X;

                return new Distance
                {
                    HorizontalDistance = lander.Situation.X - centerX,
                    VerticalDistance = lander.Situation.Y - lander.Situation.Y,
                    FullDistance = Trigonometry.GetDistance(centerX, leftSurface.Y, lander.Situation.X, lander.Situation.Y)
                };
            }

            throw new ArgumentOutOfRangeException(nameof(_surfaceList), "I guess there is no flat surface.");
        }

        public Distance DistanceFromFlatSurface(Lander lander)
        {
            for (var i = 0; i < _surfaceList.Count; i++)
            {
                var leftSurface = _surfaceList[i];
                var rightSurface = _surfaceList[i + 1];
                if (leftSurface.Y != rightSurface.Y) continue;
                var side = Side.Above;
                if (lander.Situation.X < leftSurface.X) side = Side.Left;
                if (lander.Situation.X > rightSurface.X) side = Side.Right;
                var horizontalDistance = 0;
                var fullDistance = 0.0;
                if (side == Side.Left)
                {
                    horizontalDistance = leftSurface.X - lander.Situation.X;
                    fullDistance = Trigonometry.GetDistance(leftSurface.X, leftSurface.Y, lander.Situation.X, lander.Situation.Y);
                }

                if (side == Side.Right)
                {
                    horizontalDistance = lander.Situation.X - rightSurface.X;
                    fullDistance = Trigonometry.GetDistance(rightSurface.X, rightSurface.Y, lander.Situation.X, lander.Situation.Y);
                }

                if (side == Side.Above)
                {
                    fullDistance = Trigonometry.GetDistance(rightSurface.X, rightSurface.Y, lander.Situation.X, lander.Situation.Y);
                }

                return new Distance
                {
                    HorizontalDistance = horizontalDistance,
                    VerticalDistance = lander.Situation.Y - leftSurface.Y,
                    FullDistance = fullDistance
                };
            }

            throw new ArgumentOutOfRangeException(nameof(_surfaceList), "I guess there is no flat surface.");
        }

        public string GetPotentialWinActions(Lander lander)
        {
            var puppet = lander.Clone();
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