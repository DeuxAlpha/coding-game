using CodinGame.Utilities.Maths;
using NUnit.Framework;

namespace CodinGame.Tests.Maths
{
    [TestFixture]
    public class GetNewPositionsTests
    {
        [Test]
        [TestCase(60, 0.5, 0.866)]
        [TestCase(30, 0.866, 0.5)]
        public void GetYPosition_ValidParameters_ReturnsValidYPosition(double angle, double xLength, double yResult)
        {
            Assert.That(Trigonometry.GetY(angle, xLength), Is.EqualTo(yResult));
        }

        [Test]
        [TestCase(30, 0.5, 0.866)]
        [TestCase(330, -0.5, 0.866)]
        public void GetXPosition_ValidParameters_ReturnsValidXPosition(double angle, double yLength, double xResult)
        {
            Assert.That(Trigonometry.GetX(angle, yLength), Is.EqualTo(xResult));
        }
    }
}