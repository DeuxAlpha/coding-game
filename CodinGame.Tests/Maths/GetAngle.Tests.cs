using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Structs;
using NUnit.Framework;

namespace CodinGame.Tests.Maths
{
    [TestFixture]
    public class GetAngleTests
    {
        [Test]
        [TestCase(0, 0, 5, 0, 0)]
        [TestCase(0, 0, 3, 3, 45)]
        [TestCase(0, 0, 0, 5, 90)]
        [TestCase(0, 0, -3, 3, 135)]
        [TestCase(0, 0, -5, 0, 180)]
        [TestCase(0, 0, -3, -3, 225)]
        [TestCase(0, 0, 0, -5, 270)]
        [TestCase(0, 0, 3, -3, 315)]
        public void GetAngle_ValidParameters_ReturnsAngle(int x1, int y1, int x2, int y2, double expectedAngle)
        {
            var point1 = new Point
            {
                X = x1,
                Y = y1
            };
            var point2 = new Point
            {
                X = x2,
                Y = y2
            };
            Assert.That(Trigonometry.GetAngle(point1, point2), Is.EqualTo(expectedAngle));
        }
    }
}