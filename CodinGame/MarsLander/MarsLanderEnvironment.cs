using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodinGame.MarsLander.Models;
using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Structs;

namespace CodinGame.MarsLander
{
    public class MarsLanderEnvironment
    {
        private readonly List<SurfaceZone> _surfaceZones;

        private readonly List<SurfaceElement> _surfaceElements;

        // Might have to update this in case there might be more than one landing zone.
        private SurfaceZone _landingZone;
        private int _centerLandingZoneX;

        public MarsLanderEnvironment(List<SurfaceZone> surfaceZones)
        {
            _surfaceZones = surfaceZones;
            _surfaceElements = surfaceZones.SelectMany(zone => zone.SurfaceElements).ToList();
            SetLandingZone();
        }

        private void SetLandingZone()
        {
            _landingZone = _surfaceZones.First(zone => zone.LeftY == zone.RightY);
            _centerLandingZoneX = (_landingZone.LeftX + _landingZone.RightX) / 2;
        }

        public bool LanderCrashedThroughGround(Lander lander)
        {
            var previousSituation = lander.Situations.Last();
            var angle = Trigonometry.GetAngle(
                new Point(previousSituation.X, previousSituation.Y),
                new Point(lander.Situation.X, lander.Situation.Y));
            var enteredZones = _surfaceZones
                .Where(zone => zone.LeftX >= previousSituation.X && zone.RightX < lander.Situation.X)
                .ToList();
            var currentY = previousSituation.Y;

            return false;
        }

        /// <summary>Getting the distance from the surface directly under the lander.</summary>
        public double GetDistanceFromSurface(Lander lander)
        {
            if (lander.Situation.X < 0 || lander.Situation.X > MarsLanderRules.Width) return double.MaxValue;
            return lander.Situation.Y - _surfaceElements[(int) Math.Round(lander.Situation.X)].Y;
        }

        /// <summary>Getting distance from the surface directly under the lander, but we don't need to find the surface
        /// elements under the lander.</summary>
        public static int GetDistanceFromSurface(Lander lander, SurfaceZone leftZone, SurfaceZone rightZone)
        {
            if (rightZone == null || leftZone == null)
                return int.MaxValue;
            var zoneAngle = Trigonometry
                .GetAngle(new Point(leftZone.LeftX, leftZone.LeftY), new Point(rightZone.LeftX, rightZone.LeftY));
            var landerXInZone = lander.Situation.X - leftZone.LeftX;
            var surfaceYPosition = leftZone.LeftY +
                                   Math.Round(Trigonometry.GetNewYPosition(zoneAngle, landerXInZone));
            return (int) Math.Round(lander.Situation.Y - surfaceYPosition);
        }

        public SurfaceZone GetCurrentSurface(Lander lander)
        {
            if (lander.Situation.X < 0) return null;
            if (lander.Situation.X > MarsLanderRules.Width) return null;
            return _surfaceZones
                .LastOrDefault(element => element.LeftX <= lander.Situation.X && element.RightX > lander.Situation.X);
        }

        public SurfaceZone GetRightCurrentSurface(Lander lander)
        {
            return lander.Situation.X > MarsLanderRules.Width
                ? null
                : _surfaceZones.FirstOrDefault(element => element.LeftX > lander.Situation.X);
        }

        /// <summary>Checks if lander is moving outside of map.</summary>
        public static bool IsLanderLost(Lander lander)
        {
            return lander.Situation.X < 0 || lander.Situation.X > MarsLanderRules.Width - 1;
        }

        public Distance GetDistanceFromFlatSurfaceCenter(Lander lander)
        {
            if (_landingZone == null) SetLandingZone();
            return new Distance
            {
                HorizontalDistance = Math.Abs(lander.Situation.X - _centerLandingZoneX),
                VerticalDistance = Math.Abs(lander.Situation.Y - _landingZone.LeftY),
                FullDistance =
                    Trigonometry.GetDistance(
                        new Point(_centerLandingZoneX, _landingZone.LeftY),
                        new Point(lander.Situation.X, lander.Situation.Y))
            };
        }

        public Distance GetDistanceFromFlatSurface(Lander lander)
        {
            if (_landingZone == null) SetLandingZone();
            var side = Side.Above;
            if (lander.Situation.X < _landingZone.LeftX) side = Side.Left;
            if (lander.Situation.X > _landingZone.RightX) side = Side.Right;
            var horizontalDistance = 0.0;
            var fullDistance = 0.0;
            if (side == Side.Left)
            {
                horizontalDistance = _landingZone.LeftX - lander.Situation.X;
                fullDistance = Trigonometry.GetDistance(
                    new Point(_landingZone.LeftX, _landingZone.LeftY),
                    new Point(lander.Situation.X, lander.Situation.Y));
            }

            if (side == Side.Right)
            {
                horizontalDistance = lander.Situation.X - _landingZone.RightX;
                fullDistance = Trigonometry.GetDistance(
                    new Point(_landingZone.RightX, _landingZone.RightY),
                    new Point(lander.Situation.X, lander.Situation.Y));
            }

            if (side == Side.Above)
            {
                fullDistance = Trigonometry.GetDistance(
                    new Point(lander.Situation.X, // We are above, so we do not need to move to the left or right.
                        _landingZone.LeftY), // Y is the same everywhere in the landing zone.
                    new Point(lander.Situation.X,
                        lander.Situation.Y));
            }

            return new Distance
            {
                HorizontalDistance = horizontalDistance,
                VerticalDistance =
                    lander.Situation.Y - _landingZone.LeftY, // Y is the same everywhere in the landing zone.
                FullDistance = fullDistance
            };
        }

        /// <summary>Returns the landing zone. In other words, returns the left and right surface elements that make out
        /// the landing zone.</summary>
        public SurfaceZone GetLandingZone()
        {
            if (_landingZone != null)
                return _landingZone; // Should always be true, as the landing zone gets set in the constructor.
            // However, for sanity's sake, we are going to reset them here in case this didn't happen. This should never
            // be called, though.
            SetLandingZone();
            return _landingZone;
        }
    }
}