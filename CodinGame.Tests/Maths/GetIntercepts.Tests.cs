using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Structs;
using NUnit.Framework;

namespace CodinGame.Tests.Maths
{
    [TestFixture]
    public class GetInterceptsTests
    {
        [Test]
        [TestCase(1, 1, 3, 3, 2, 2)]
        [TestCase(0, 5, 0, -5, 0, 0)]
        [TestCase(3, 3, 1, 1, 2, 2)]
        public void GetXIntercept_ValidParameters_ReturnsIntercept(
            double x1,
            double y1,
            double x2,
            double y2,
            double yLevel,
            double expectedResult)
        {
            Assert.That(Trigonometry.GetXIntersect(new Point{X = x1,Y = y1}, new Point {X = x2, Y = y2}, yLevel), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(1, 1, 3, 3, 2, 2)]
        [TestCase(5, 0, -5, 0, 0, 0)]
        [TestCase(3, 0, -5, 0, 0, 0)]
        [TestCase(3, 3, 1, 1, 2, 2)]
        public void GetYIntercept_ValidParameters_ReturnsIntercept(
            double x1,
            double y1,
            double x2,
            double y2,
            double yLevel,
            double expectedResult)
        {
            Assert.That(Trigonometry.GetYIntersect(new Point(x1, y1), new Point(x2, y2), yLevel), Is.EqualTo(expectedResult));
        }
    }
}