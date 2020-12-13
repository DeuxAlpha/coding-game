using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.Utilities.Graphs;
using CodinGame.Utilities.Graphs.Grids;
using NUnit.Framework;

namespace CodinGame.Tests.Graphs
{
    [TestFixture]
    public class AStarTests
    {
        [Test]
        public void TestAStar_ValidParameters_ReturnAStarNodes()
        {
            var graph = new Grid(8, 8);

            var nodes = graph.GetAStarNodes("00", "77");

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("77"));
                Assert.That(nodes.First().Id, Is.EqualTo("10"));
            });
        }

        [Test]
        public void TestAStar_LimitedMovement_ReturnsAStarNodes()
        {
            var grid = new Grid(8, 8);

            grid.RemoveConnection("70", "71");
            grid.RemoveConnection("60", "61");
            // Algorithm now goes down at 50 to 51, then to 71, and then down.

            var nodes = grid.GetAStarNodes("00", "77");

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("77"));
                Assert.That(nodes.First().Id, Is.EqualTo("10"));
            });
        }
    }
}