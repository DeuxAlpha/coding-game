using CodinGame.Utilities.Maths;
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
            Assert.That(Trigonometry.GetAngle(x1, y1, x2, y2), Is.EqualTo(expectedAngle));
        }
    }
}