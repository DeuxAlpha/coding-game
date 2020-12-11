using System;
using System.Diagnostics;
using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Enums;
using CodinGame.Utilities.Maths.Models;
using NUnit.Framework;
using Double = CodinGame.Utilities.Extensions.Double;

namespace CodinGame.Tests.Maths
{
    [TestFixture]
    public class GetIntersectionsTests
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
            var sw = Stopwatch.StartNew();
            var xIntersect = Trigonometry.GetXIntersect(new Point {X = x1, Y = y1}, new Point {X = x2, Y = y2}, yLevel);
            Console.WriteLine(sw.Elapsed);
            Assert.That(xIntersect,
                Is.EqualTo(expectedResult));
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
            var sw = Stopwatch.StartNew();
            var yIntersect = Trigonometry.GetYIntersect(new Point(x1, y1), new Point(x2, y2), yLevel);
            Console.WriteLine(sw.Elapsed);
            Assert.That(yIntersect,
                Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(0, 0, 5, 5, 0, 5, 5, 0, IntersectionType.Point, 2.5, 2.5)]
        [TestCase(3, 0, 3, 4, 0, 5, 5, 5, IntersectionType.None, null, null)]
        [TestCase(0, 0, 2, 0, 1, 0, 3, 0, IntersectionType.CollinearOverlapping, null, null)]
        [TestCase(10, 10, 20, -10, Double.SmallValue, 0, Double.BigValue, 0, IntersectionType.Point, 15, 0)]
        [TestCase(0, 5, 0, -5, -5, 0, 5, 0, IntersectionType.Point, 0, 0)]
        public void GetIntersections_ValidParameters_GetIntersection(
            double x1,
            double y1,
            double x2,
            double y2,
            double x3,
            double y3,
            double x4,
            double y4,
            IntersectionType expectedIntersectionType,
            double? expectedIntersectX,
            double? expectedIntersectY)
        {
            var vector1 = new Vector(new Point(x1, y1), new Point(x2, y2));
            var vector2 = new Vector(new Point(x3, y3), new Point(x4, y4));
            var sw = Stopwatch.StartNew();
            var intersection = Trigonometry.GetIntersection(vector1, vector2);
            Console.WriteLine(sw.Elapsed);
            Assert.Multiple(() =>
            {
                Assert.That(intersection.IntersectionType, Is.EqualTo(expectedIntersectionType));
                Assert.That(intersection.X, Is.EqualTo(expectedIntersectX));
                Assert.That(intersection.Y, Is.EqualTo(expectedIntersectY));
            });
        }
    }
}