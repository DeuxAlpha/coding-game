using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodinGame.GreatEscape.v2.Models;
using CodinGame.GreatEscape.v2.Models.Enums;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Graphs.Grids;

namespace CodinGame.GreatEscape.v2
{
    public class GreatEscapeManager
    {
        public static void Play()
        {
            var inputs = Console.ReadLine()!.Split(' ');
            var width = int.Parse(inputs[0]); // width of the board
            var height = int.Parse(inputs[1]); // height of the board
            var environment = new GreatEscapeEnvironment(height, width);
            var playerCount = int.Parse(inputs[2]); // number of players (2 or 3)
            var myId = int.Parse(inputs[3]); // id of my player (0 = 1st player, 1 = 2nd player, ...)
            var dragons = new List<Dragon>();
            for (var playerId = 0; playerId < playerCount; playerId++)
            {
                dragons.Add(new Dragon(playerId == myId ? PlayerType.Player : PlayerType.Opponent));
            }

            var targetDirection = myId switch
            {
                0 => TargetDirection.Right,
                1 => TargetDirection.Left,
                2 => TargetDirection.Down,
                _ => TargetDirection.Right
            };

            var grid = new Grid(width, height);
            var round = 0;

            // game loop
            while (true)
            {
                // TODO: Use proper models
                int? playerCoordsX = null;
                int? playerCoordsY = null;
                int? firstOpponentId = null;
                int? firstOpponentCoordsX = null;
                int? firstOpponentCoordsY = null;
                int? secondOpponentId = null;
                int? secondOpponentCoordsX = null;
                int? secondOpponentCoordsY = null;

                for (var i = 0; i < playerCount; i++)
                {
                    inputs = Console.ReadLine()!.Split(' ');
                    var x = int.Parse(inputs[0]); // x-coordinate of the player
                    var y = int.Parse(inputs[1]); // y-coordinate of the player
                    if (myId == i)
                    {
                        playerCoordsX = x;
                        playerCoordsY = y;
                    }
                    else
                    {
                        if (firstOpponentCoordsX == null || firstOpponentCoordsY == null)
                        {
                            firstOpponentCoordsX = x;
                            firstOpponentCoordsY = y;
                            firstOpponentId = i;
                        }
                        else
                        {
                            secondOpponentCoordsX = x;
                            secondOpponentCoordsY = y;
                            secondOpponentId = i;
                        }
                    }

                    var wallsLeft = int.Parse(inputs[2]); // number of walls available for the player
                    var cell = environment.Map.GetCell(x, y);
                    // dragons[i].Update(cell, wallsLeft);
                }

                var wallCount = int.Parse(Console.ReadLine()!); // number of walls on the board
                for (var i = 0; i < wallCount; i++)
                {
                    inputs = Console.ReadLine()!.Split(' ');
                    var wallX = int.Parse(inputs[0]); // x-coordinate of the wall
                    var wallY = int.Parse(inputs[1]); // y-coordinate of the wall
                    var wallOrientation = inputs[2]; // wall orientation ('H' or 'V')
                    if (wallOrientation == "H")
                    {
                        grid.RemoveConnection($"{wallX}-{wallY}", $"{wallX}-{wallY - 1}");
                        grid.RemoveConnection($"{wallX + 1}-{wallY}", $"{wallX + 1}-{wallY - 1}");
                    }
                    else
                    {
                        grid.RemoveConnection($"{wallX}-{wallY}", $"{wallX - 1}-{wallY}");
                        grid.RemoveConnection($"{wallX}-{wallY + 1}", $"{wallX - 1}-{wallY + 1}");
                    }
                }



                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                var target = targetDirection switch
                {
                    TargetDirection.Right => $"7-{playerCoordsY}",
                    TargetDirection.Left => $"0-{playerCoordsY}",
                    TargetDirection.Down => $"{playerCoordsX}-7",
                    TargetDirection.Up => $"{playerCoordsX}-0"
                };

                var playerCoordinates = $"{playerCoordsX}-{playerCoordsY}";
                var sw = Stopwatch.StartNew();
                Logger.Log("Player Coords", playerCoordinates);
                Logger.Log("Target", target);
                var travelNodes = grid.GetAStarNodesWithList(playerCoordinates, target);
                Logger.Log(sw.Elapsed.ToString());
                var firstNode = travelNodes.FirstOrDefault();
                var action = targetDirection.ToString("G").ToUpper();
                if (firstNode != null)
                {
                    if (firstNode.X > playerCoordsX)
                        action = "RIGHT";
                    if (firstNode.X < playerCoordsX)
                        action = "LEFT";
                    if (firstNode.Y > playerCoordsY)
                        action = "DOWN";
                    if (firstNode.Y < playerCoordsY)
                        action = "UP";
                }

                // Simply check if our count of nodes is less than that of the opponents, if it is not, set wall in front of
                // opponents with least amount of remaining nodes.

                // action: LEFT, RIGHT, UP, DOWN or "putX putY putOrientation" to place a wall
                // Place a wall in front of opponent if we're not first (other wise we will loose.
                if (round == 0 && myId != 0)
                {
                    // Checking for target direction. If we're going right, we know the opponent goes left.
                    if (targetDirection == TargetDirection.Right)
                    {
                        var targetCellX = firstOpponentCoordsX - 2;
                        var targetCellY = firstOpponentCoordsY;
                        if (targetCellY == 7) targetCellY -= 1;
                        if (targetCellY + 1 == playerCoordsY) targetCellY -= 1;
                        Actions.Commit($"{targetCellX} {targetCellY} V");
                    }
                    else
                    {
                        var targetCellX = firstOpponentCoordsX + 2;
                        var targetCellY = firstOpponentCoordsY;
                        if (targetCellY == 7) targetCellY -= 1;
                        if (targetCellY + 1 == playerCoordsY) targetCellY -= 1;
                        Actions.Commit($"{targetCellX} {targetCellY} V");
                    }
                }

                if (round == 1 && myId != 0)
                {
                    // Since we spent a round blocking the first player, we also need to spend some time to block the upper player.
                    if (targetDirection == TargetDirection.Left)
                    {
                        // If we're going left, the second player is going down.
                        var targetCellX = secondOpponentCoordsX;
                        var targetCellY = secondOpponentCoordsY + 2;
                        if (targetCellX == 7) targetCellX -= 1;
                        if (targetCellX + 1 == playerCoordsX) targetCellX -= 1;
                        Actions.Commit($"{targetCellX} {targetCellY} H");
                    }

                    if (targetDirection == TargetDirection.Down)
                    {
                        var targetCellX = secondOpponentCoordsX - 2;
                        var targetCellY = secondOpponentCoordsY;
                        if (targetCellY == 7) targetCellY -= 1;
                        if (targetCellY + 1 == playerCoordsY) targetCellY -= 1;
                        Actions.Commit($"{targetCellX} {targetCellY} V");
                    }
                }
                else
                {
                    Actions.Commit(action);
                }

                round += 1;
            }
        }
    }
}