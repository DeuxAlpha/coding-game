using System;
using System.Diagnostics;
using CodinGame.Utilities.Graphs.Grids;
using NUnit.Framework;

namespace CodinGame.Tests.Graphs
{
    [TestFixture]
    public class GridTests
    {
        [Test]
        public void CloneGrid_BigGrid_ClonesGrid()
        {
            var sw = Stopwatch.StartNew();
            var grid = new Grid(50, 50);
            Console.WriteLine(sw.Elapsed);

            sw = Stopwatch.StartNew();
            var gridClone = grid.Clone();
            Console.WriteLine(sw.Elapsed);

            Assert.That(gridClone.Nodes, Has.Count.EqualTo(grid.Nodes.Count));
            Assert.That(gridClone.Edges, Has.Count.EqualTo(grid.Edges.Count));
        }
    }
}