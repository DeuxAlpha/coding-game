using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.Utilities.Game;
using CodinGame.VoxCodei.Models;

namespace CodinGame.VoxCodei
{
    public static class VoxCodeiEntry
    {
        public static void Enter()
        {
            var inputs = Console.ReadLine()!.Split(' ');
            var width = int.Parse(inputs[0]); // width of the firewall grid
            var height = int.Parse(inputs[1]); // height of the firewall grid
            var map = new SurveillanceMap(width, height);
            for (var heightIndex = 0; heightIndex < height; heightIndex++)
            {
                var mapRow = Console.ReadLine() ?? ""; // one line of the firewall grid
                var rowArray = mapRow.ToCharArray();
                for (var colIndex = 0; colIndex < rowArray.Length; colIndex++)
                {
                    var character = rowArray[colIndex];
                    var node = new Node(colIndex, heightIndex, character);
                    map.SetNode(colIndex, heightIndex, node);
                }
            }

            // game loop
            while (true)
            {
                map.DrawMap();

                inputs = Console.ReadLine()!.Split(' ');
                var rounds = int.Parse(inputs[0]); // number of rounds left before the end of the game
                var bombs = int.Parse(inputs[1]); // number of bombs left

                var activeNodes = map.GetActiveNodes();
                var potentialNodes = new List<NodeCountTracker>();
                foreach (var activeNode in activeNodes)
                {
                    var targetNodes = map.GetEmptyNodesAroundNode(activeNode);
                    foreach (var targetNode in targetNodes)
                    {
                        var existingNode = potentialNodes.FirstOrDefault(node => node.Node == targetNode);
                        if (existingNode != null) existingNode.NodeCount += 1;
                        potentialNodes.Add(new NodeCountTracker(targetNode));
                    }
                }

                var bombNode = potentialNodes.OrderByDescending(node => node.NodeCount).First().Node;

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                Actions.Commit($"{bombNode.X} {bombNode.Y}");
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}