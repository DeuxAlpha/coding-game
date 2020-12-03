using System;
using CodinGame.Utilities.Random;
using NUnit.Framework;

namespace CodinGame.Tests.Randoms
{
    [TestFixture]
    public class RandomizerTests
    {
        [Test]
        [Repeat(25)]
        public void TestRandomizedDouble_ValidParameters_ReturnsRandomizedDouble()
        {
            Assert.That(Randomizer.GetValueBetween(0.001, 0.002, 3), Is.EqualTo(0.001).Or.EqualTo(0.002));
        }
    }
}