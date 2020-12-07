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
        public IEnumerable<SurfaceElement> Surface => _surfaceList;

        public MarsLanderEnvironment(List<SurfaceElement> surfaceList)
        {
            _surfaceList = surfaceList;
        }

        /// <summary>Getting the distance from the surface directly under the lander.</summary>
        public int GetDistanceFromSurface(Lander lander)
        {
            var rightZoneUnderLander =
                _surfaceList.FirstOrDefault(element =>
                    element.X >= lander.Situation.X && lander.Situation.X <= element.X);
            var rightZoneUnderLanderIndex = _surfaceList.IndexOf(rightZoneUnderLander);
            var leftZoneUnderLander = _surfaceList.ElementAtOrDefault(rightZoneUnderLanderIndex - 1);
            if (rightZoneUnderLander == null || leftZoneUnderLander == null)
                return int.MaxValue;
            var zoneAngle = Trigonometry
                .GetAngle(leftZoneUnderLander.X, leftZoneUnderLander.Y, rightZoneUnderLander.X, rightZoneUnderLander.Y);
            var landerXInZone = lander.Situation.X - leftZoneUnderLander.X;
            var surfaceYPosition = leftZoneUnderLander.Y +
                                   Math.Round(Trigonometry.GetNewYPosition(zoneAngle, landerXInZone));
            return (int) Math.Round(lander.Situation.Y - surfaceYPosition);
        }

        /// <summary>Checks if lander is moving outside of map.</summary>
        public static bool IsLanderLost(Lander lander)
        {
            return lander.Situation.X < 0 || lander.Situation.X > MarsLanderRules.Width;
        }

        public Distance GetDistanceFromFlatSurfaceCenter(Lander lander)
        {
            for (var i = 0; i < _surfaceList.Count; i++)
            {
                var leftSurface = _surfaceList[i];
                var rightSurface = _surfaceList[i + 1];
                if (leftSurface.Y != rightSurface.Y) continue;
                var centerX = (leftSurface.X + rightSurface.X) / 2;

                return new Distance
                {
                    HorizontalDistance = Math.Abs(lander.Situation.X - centerX),
                    VerticalDistance = Math.Abs(lander.Situation.Y - leftSurface.Y),
                    FullDistance =
                        Trigonometry.GetDistance(centerX, leftSurface.Y, lander.Situation.X, lander.Situation.Y)
                };
            }

            throw new ArgumentOutOfRangeException(nameof(_surfaceList), "I guess there is no flat surface.");
        }

        public Distance GetDistanceFromFlatSurface(Lander lander)
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
                    fullDistance = Trigonometry.GetDistance(leftSurface.X, leftSurface.Y, lander.Situation.X,
                        lander.Situation.Y);
                }

                if (side == Side.Right)
                {
                    horizontalDistance = lander.Situation.X - rightSurface.X;
                    fullDistance = Trigonometry.GetDistance(rightSurface.X, rightSurface.Y, lander.Situation.X,
                        lander.Situation.Y);
                }

                if (side == Side.Above)
                {
                    fullDistance = Trigonometry.GetDistance(rightSurface.X, rightSurface.Y, lander.Situation.X,
                        lander.Situation.Y);
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
            var distanceFromFlatSurface = GetDistanceFromFlatSurface(puppet);
            if (Math.Abs(distanceFromFlatSurface.HorizontalDistance) > 0)
                return null;
            if (Math.Abs(distanceFromFlatSurface.VerticalDistance) < 50)
                return "0 4";
            return puppet.LimitMomentum();
        }

        /// <summary>Returns the landing zone. In other words, returns the left and right surface elements that make out
        /// the landing zone.</summary>
        public IEnumerable<SurfaceElement> GetLandingZone()
        {
            for (var i = 0; i < _surfaceList.Count; i++)
            {
                var leftSurface = _surfaceList[i];
                var rightSurface = _surfaceList[i + 1];
                if (leftSurface.Y != rightSurface.Y) continue;
                return new[] {leftSurface, rightSurface};
            }

            throw new ArgumentOutOfRangeException(nameof(_surfaceList));
        }
    }
}