using System;
using CodinGame.Utilities.Maths;
using NUnit.Framework;

namespace CodinGame.Tests.Maths
{
    [TestFixture]
    public class GetSpeedFractionsTests
    {
        [Test]
        [TestCase(0, 1)]
        [TestCase(30, 0.866)]
        [TestCase(45, 0.707)]
        [TestCase(60, 0.500)]
        [TestCase(90, 0.000)]
        [TestCase(180, -1)]
        public void GetHorizontalSpeed_ValidParameters_ReturnsValidSpeed(int angle, double result)
        {
            Assert.That(Trigonometry.GetHorizontalSpeedFraction(angle), Is.EqualTo(Math.Round(result, 3)));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(30, 0.5)]
        [TestCase(45, 0.707)]
        [TestCase(60, 0.866)]
        [TestCase(90, 1)]
        [TestCase(180, 0)]
        [TestCase(270, -1)]
        public void GetVerticalSpeed_ValidParameters_ReturnsValidSpeed(int angle, double result)
        {
            Assert.That(Trigonometry.GetVerticalSpeedFraction(angle), Is.EqualTo(Math.Round(result, 3)));
        }
    }
}