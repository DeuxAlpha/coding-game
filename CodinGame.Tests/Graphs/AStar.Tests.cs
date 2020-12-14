using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var sw = Stopwatch.StartNew();
            var nodes = graph.GetAStarNodesWithHeap("0-0", "7-7");
            Console.WriteLine(sw.Elapsed);

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("7-7"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-0"));
            });
        }

        [Test]
        public void TestAStar_LimitedMovement_ReturnsAStarNodes()
        {
            var grid = new Grid(8, 8);

            grid.RemoveConnection("7-0", "7-1");
            grid.RemoveConnection("6-0", "6-1");
            // Algorithm now goes down at 50 to 51, then to 71, and then down.

            var sw = Stopwatch.StartNew();
            var nodes = grid.GetAStarNodesWithHeap("0-0", "7-7");
            Console.WriteLine(sw.Elapsed);

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("7-7"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-0"));
            });
        }

        [Test]
        public void TestAStar_BigGrid_ReturnsAStarNodes()
        {
            var sw = Stopwatch.StartNew();
            var grid = new Grid(100, 100);
            Console.WriteLine(sw.Elapsed);

            sw = Stopwatch.StartNew();
            var nodes = grid.GetAStarNodesWithHeap("0-0", "99-99");
            Console.WriteLine(sw.Elapsed);

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("99-99"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-0"));
            });
        }

        [Test]
        public void TestAStarWithList_BigGrid_ReturnAStarNodes()
        {
            var sw = Stopwatch.StartNew();
            var graph = new Grid(100, 100);
            Console.WriteLine(sw.Elapsed);

            sw = Stopwatch.StartNew();
            var nodes = graph.GetAStarNodesWithList("0-0", "99-99");
            Console.WriteLine(sw.Elapsed);

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("99-99"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-0"));
            });
        }

        [Test]
        public void TestAStarWithList_ValidParameters_ReturnAStarNodes()
        {
            var graph = new Grid(8, 8);

            var sw = Stopwatch.StartNew();
            var nodes = graph.GetAStarNodesWithList("0-0", "7-7");
            Console.WriteLine(sw.Elapsed);

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("7-7"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-0"));
            });
        }

        [Test]
        public void TestAStarWithList_LimitedMovement_ReturnsAStarNodes()
        {
            var grid = new Grid(8, 8);

            grid.RemoveConnection("7-0", "7-1");
            grid.RemoveConnection("6-0", "6-1");
            // Algorithm now goes down at 50 to 51, then to 71, and then down.

            var sw = Stopwatch.StartNew();
            var nodes = grid.GetAStarNodesWithList("0-0", "7-7");
            Console.WriteLine(sw.Elapsed);

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(14));
                Assert.That(nodes.Last().Id, Is.EqualTo("7-7"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-0"));
            });
        }

        [Test]
        public void TestAStar_CutOffAtBottom_ReturnsAStarNodes()
        {
            var grid = new Grid(9, 9);

            grid.RemoveConnection("1-7", "2-7");
            grid.RemoveConnection("1-8", "2-8");

            var nodes = grid.GetAStarNodesWithList("1-7", "7-7");

            Assert.Multiple(() =>
            {
                Assert.That(nodes, Has.Count.EqualTo(8));
                Assert.That(nodes.Last().Id, Is.EqualTo("7-7"));
                Assert.That(nodes.First().Id, Is.EqualTo("1-6"));
            });
        }

        [Test]
        public void TestAStar_AllPathsClosed_ReturnsEmptyNodeList()
        {
            var grid = new Grid(4, 4);

            grid.RemoveConnection("1-0", "2-0");
            grid.RemoveConnection("1-1", "2-1");
            grid.RemoveConnection("1-2", "2-2");
            grid.RemoveConnection("1-3", "2-3");

            var nodes = grid.GetAStarNodesWithList("0-0", "3-0");

            Assert.That(nodes, Is.Empty);
        }
    }
}