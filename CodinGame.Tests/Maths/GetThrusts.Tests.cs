using System;
using CodinGame.Utilities.Maths;
using NUnit.Framework;

namespace CodinGame.Tests.Maths
{
    [TestFixture]
    public class GetThrustsTests
    {
        [Test]
        [TestCase(10, 0, 0, 125, 5, 0, 0, 175, 0, 0, 0, 0, 0, 0, 0, 0, 1, 15, 90, 4)]
        [TestCase(3.711, 0, 2500, 2700, 0, 0, 0, 4750, 100, 0, 0, 0, 750, 5, 20, 40, 1, 15, 90, 4)]
        public void GetThrustActions_ValidParameters_ReturnsThrustActions(
            double gravity,
            double currentRotation,
            double currentX,
            double currentY,
            double currentXSpeed,
            double currentYSpeed,
            double desiredRotation,
            double desiredX,
            double desiredY,
            double desiredXSpeed,
            double desiredYSpeed,
            double rotationTolerance,
            double xTolerance,
            double yTolerance,
            double xSpeedTolerance,
            double ySpeedTolerance,
            double maxPowerChange,
            double maxRotationChange,
            double maxRotation,
            double maxPower)
        {
            var object2D = new Object2D();
            var actions = Trigonometry.GetRequiredThrust(new ThrustRequest
            {
                Gravity = gravity,
                CurrentRotation = currentRotation,
                CurrentX = currentX,
                CurrentY = currentY,
                CurrentXSpeed = currentXSpeed,
                CurrentYSpeed = currentYSpeed,
                DesiredRotation = desiredRotation,
                DesiredX = desiredX,
                DesiredY = desiredY,
                DesiredYSpeed = desiredYSpeed,
                DesiredXSpeed = desiredXSpeed,
                XSpeedTolerance = xSpeedTolerance,
                YSpeedTolerance = ySpeedTolerance,
                MaxPowerChange = maxPowerChange,
                MaxRotationChange = maxRotationChange,
                MaxRotation = maxRotation,
                MaxPower = maxPower,
                XTolerance = xTolerance,
                YTolerance = yTolerance
            });
            foreach (var action in actions)
            {
                object2D.ApplyThrust(action, 10);
            }

            Assert.Multiple(() =>
            {
                Assert.That(Math.Abs(object2D.State.Rotation - desiredRotation),
                    Is.LessThanOrEqualTo(rotationTolerance));
                Assert.That(Math.Abs(object2D.State.X - desiredX), Is.LessThanOrEqualTo(xTolerance));
                Assert.That(Math.Abs(object2D.State.Y - desiredY), Is.LessThanOrEqualTo(yTolerance));
                Assert.That(Math.Abs(object2D.State.XSpeed - desiredXSpeed), Is.LessThanOrEqualTo(xSpeedTolerance));
                Assert.That(Math.Abs(object2D.State.YSpeed - desiredYSpeed), Is.LessThanOrEqualTo(ySpeedTolerance));
            });
        }
    }
}