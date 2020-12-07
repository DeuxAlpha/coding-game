using System;
using System.Collections.Generic;
using System.Linq;

namespace CodinGame.Utilities.Maths
{
    public static partial class Trigonometry
    {
        // TODO: Find a way to calculate applied force to reach a destination with a specific speed.
        /// <summary>This method provides a collection of required angles and thrust to reach the destination based on
        /// the provided parameters. Bear in mind, though, that it does not check for passing through any surface
        /// elements (i.e. crashing).</summary>
        public static IEnumerable<Thrust> GetRequiredThrust(ThrustRequest request)
        {
            var thrustActions = new List<Thrust>();
            var currentObject = new Object2D(new Object2DMotion
            {
                Rotation = request.CurrentRotation,
                X = request.CurrentX,
                Y = request.CurrentY,
                XSpeed = request.CurrentXSpeed,
                YSpeed = request.CurrentYSpeed
            });
            while (Math.Abs(currentObject.State.X - request.DesiredX) > request.XTolerance ||
                   Math.Abs(currentObject.State.Y - request.DesiredY) > request.YTolerance ||
                   Math.Abs(currentObject.State.XSpeed - request.DesiredXSpeed) > request.XSpeedTolerance ||
                   Math.Abs(currentObject.State.YSpeed - request.DesiredYSpeed) > request.YSpeedTolerance ||
                   Math.Abs(currentObject.State.Rotation - request.DesiredRotation) > request.RotationTolerance)
            {
                var thrust = new Thrust();
                var freeFallInformation = GetFreeFallInformation(currentObject, request);
                if (freeFallInformation.FinalXOffset > 0)
                {
                    // Burn against movement.

                }
                else
                {
                    // Burn in support of movement.

                }
            }

            return thrustActions;
        }

        /// <summary>Calculates where, if no further force was applied, the object would end up on the X axis in
        /// comparison to where we want it, as well as how long it will take.</summary>
        private static FreeFallInformation GetFreeFallInformation(Object2D object2D, ThrustRequest thrustRequest)
        {
            var clone = object2D.Clone();
            var ticks = 0;
            while (clone.State.Y > thrustRequest.DesiredY)
            {
                clone.ApplyThrust(new Thrust(0, 0), thrustRequest.Gravity);
                ticks += 1;
            }

            var firstState = clone.StateHistory.FirstOrDefault() ?? new Object2DMotion();
            var secondState = clone.StateHistory.ElementAtOrDefault(1) ?? new Object2DMotion();
            var initialAngle = GetAngle(firstState.X, firstState.Y, secondState.X, secondState.Y);
            var previousState = clone.StateHistory.LastOrDefault() ?? new Object2DMotion();
            var xIntersect = GetXIntersect(previousState.X, previousState.Y, clone.State.X, clone.State.Y,
                thrustRequest.DesiredY);
            var finalAngle = GetAngle(previousState.X, previousState.Y, clone.State.X, clone.State.Y);
            return new FreeFallInformation
            {
                FinalX = xIntersect,
                FinalXOffset = clone.State.X - xIntersect,
                Ticks = ticks,
                FinalAngle = finalAngle,
                InitialAngle = initialAngle
            };
        }
    }
}