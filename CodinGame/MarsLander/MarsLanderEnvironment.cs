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
        private List<SurfaceZone> _surfaceList;
        public IEnumerable<SurfaceZone> Surface => _surfaceList;
        // Might have to update this in case there might be more than one landing zone.
        private SurfaceZone _leftLandingZone;
        private SurfaceZone _rightLandingZone;
        private int _centerLandingZoneX;

        public MarsLanderEnvironment(List<SurfaceZone> surfaceList)
        {
            _surfaceList = surfaceList;
            SetLandingZone();
        }

        private void SetLandingZone()
        {
            for (var i = 0; i < _surfaceList.Count; i++)
            {
                var leftSurface = _surfaceList[i];
                var rightSurface = _surfaceList[i + 1];
                if (leftSurface.LeftY != rightSurface.LeftY) continue;
                _leftLandingZone = leftSurface;
                _rightLandingZone = rightSurface;
                _centerLandingZoneX = (leftSurface.LeftX + rightSurface.LeftX) / 2;
                return;
            }
        }

        /// <summary>Getting the distance from the surface directly under the lander.</summary>
        public int GetDistanceFromSurface(Lander lander)
        {
            var rightZoneUnderLander =
                _surfaceList.First(element =>
                    element.LeftX >= lander.Situation.X && lander.Situation.X <= element.LeftX);
            var rightZoneUnderLanderIndex = _surfaceList.IndexOf(rightZoneUnderLander);
            var leftZoneUnderLander = _surfaceList.ElementAt(rightZoneUnderLanderIndex - 1);
            return GetDistanceFromSurface(lander, leftZoneUnderLander, rightZoneUnderLander);
        }

        /// <summary>Getting distance from the surface directly under the lander, but we don't need to find the surface
        /// elements under the lander.</summary>
        public static int GetDistanceFromSurface(Lander lander, SurfaceZone leftZone, SurfaceZone rightZone)
        {
            if (rightZone == null || leftZone == null)
                return int.MaxValue;
            var zoneAngle = Trigonometry
                .GetAngle(leftZone.LeftX, leftZone.LeftY, rightZone.LeftX, rightZone.LeftY);
            var landerXInZone = lander.Situation.X - leftZone.LeftX;
            var surfaceYPosition = leftZone.LeftY +
                                   Math.Round(Trigonometry.GetNewYPosition(zoneAngle, landerXInZone));
            return (int) Math.Round(lander.Situation.Y - surfaceYPosition);
        }

        public SurfaceZone GetLeftCurrentSurface(Lander lander)
        {
            return lander.Situation.X < 0 ? null : _surfaceList.Last(element => element.LeftX < lander.Situation.X);
        }

        public SurfaceZone GetRightCurrentSurface(Lander lander)
        {
            return lander.Situation.X > MarsLanderRules.Width
                ? null
                : _surfaceList.First(element => element.LeftX > lander.Situation.X);
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
                if (leftSurface.LeftY != rightSurface.LeftY) continue;
                var centerX = (leftSurface.LeftX + rightSurface.LeftX) / 2;

                return new Distance
                {
                    HorizontalDistance = Math.Abs(lander.Situation.X - centerX),
                    VerticalDistance = Math.Abs(lander.Situation.Y - leftSurface.LeftY),
                    FullDistance =
                        Trigonometry.GetDistance(centerX, leftSurface.LeftY, lander.Situation.X, lander.Situation.Y)
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
                if (leftSurface.LeftY != rightSurface.LeftY) continue;
                var side = Side.Above;
                if (lander.Situation.X < leftSurface.LeftX) side = Side.Left;
                if (lander.Situation.X > rightSurface.LeftX) side = Side.Right;
                var horizontalDistance = 0;
                var fullDistance = 0.0;
                if (side == Side.Left)
                {
                    horizontalDistance = leftSurface.LeftX - lander.Situation.X;
                    fullDistance = Trigonometry.GetDistance(leftSurface.LeftX, leftSurface.LeftY, lander.Situation.X,
                        lander.Situation.Y);
                }

                if (side == Side.Right)
                {
                    horizontalDistance = lander.Situation.X - rightSurface.LeftX;
                    fullDistance = Trigonometry.GetDistance(rightSurface.LeftX, rightSurface.LeftY, lander.Situation.X,
                        lander.Situation.Y);
                }

                if (side == Side.Above)
                {
                    fullDistance = Trigonometry.GetDistance(rightSurface.LeftX, rightSurface.LeftY, lander.Situation.X,
                        lander.Situation.Y);
                }

                return new Distance
                {
                    HorizontalDistance = horizontalDistance,
                    VerticalDistance = lander.Situation.Y - leftSurface.LeftY,
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
        public IEnumerable<SurfaceZone> GetLandingZone()
        {
            if (_leftLandingZone != null || _rightLandingZone != null) // Should always be true, as the landing zone gets set in the constructor.
                return new[] {_leftLandingZone, _rightLandingZone};
            // However, for sanity's sake, we are going to reset them here in case this didn't happen. This should never
            // be called, though.
            SetLandingZone();
            return new[] {_leftLandingZone, _rightLandingZone};
        }
    }
}