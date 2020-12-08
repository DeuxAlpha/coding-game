using System.Collections.Generic;
using System.Linq;

namespace CodinGame.Utilities.Maths
{
    public class Object2D
    {
        public Object2DMotion State { get; set; }
        public List<Object2DMotion> StateHistory { get; set; }= new List<Object2DMotion>();

        public Object2D()
        {

        }

        public Object2D(Object2DMotion initialState)
        {
            State = initialState.Clone();
        }

        public Object2D Clone()
        {
            return new Object2D
            {
                State = State.Clone(),
                StateHistory = StateHistory.ToList()
            };
        }

        /// <summary>Moves the object. Gravity needs to be positive (gets reverted within the method).</summary>
        public void ApplyThrust(
            Thrust thrust,
            double gravity,
            ZeroDegreesDirection zeroDegreesDirection = ZeroDegreesDirection.Top)
        {
            StateHistory.Add(State.Clone());
            State.XSpeed += Trigonometry.GetHorizontalSpeedFraction(thrust.Rotation, zeroDegreesDirection) * thrust.Power;
            State.YSpeed += Trigonometry.GetVerticalSpeedFraction(thrust.Rotation, zeroDegreesDirection) * thrust.Power -
                      gravity;
            State.Power = thrust.Power;
            State.X += State.XSpeed;
            State.Y += State.YSpeed;
            State.Rotation = thrust.Rotation;
        }
    }
}