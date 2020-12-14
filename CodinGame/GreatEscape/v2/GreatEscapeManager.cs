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
            var playerCount = int.Parse(inputs[2]); // number of players (2 or 3)
            var myId = int.Parse(inputs[3]); // id of my player (0 = 1st player, 1 = 2nd player, ...)
            var dragons = new List<Dragon>();
            for (var playerId = 0; playerId < playerCount; playerId++)
            {
                dragons.Add(new Dragon(playerId == myId ? PlayerType.Player : PlayerType.Opponent, playerId));
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
                for (var i = 0; i < playerCount; i++)
                {
                    inputs = Console.ReadLine()!.Split(' ');
                    var x = int.Parse(inputs[0]); // x-coordinate of the player
                    var y = int.Parse(inputs[1]); // y-coordinate of the player
                    var wallsLeft = int.Parse(inputs[2]); // number of walls available for the player
                    dragons[i].Update(x, y, wallsLeft);
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

                var player = dragons.First(dragon => dragon.Id == myId);

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                var target = targetDirection switch
                {
                    TargetDirection.Right => $"8-{player.Y}",
                    TargetDirection.Left => $"0-{player.Y}",
                    TargetDirection.Down => $"{player.X}-8",
                    TargetDirection.Up => $"{player.X}-0"
                };

                var playerCoordinates = $"{player.X}-{player.Y}";
                Logger.Log("Player Coords", playerCoordinates);
                Logger.Log("Target", target);

                var playerInformation = new PlayerInformation
                {
                    Dragon = player,
                    Nodes = grid.GetAStarNodesWithList(playerCoordinates, target)
                };

                var otherPlayerInformation = dragons
                    .Where(dragon => dragon.Id != myId && dragon.State == DragonState.Alive)
                    .Select(dragon => new PlayerInformation
                    {
                        Dragon = dragon,
                        Nodes = GetDragonTravelNodes(dragon, grid).ToList()
                    })
                    .OrderBy(dragon => dragon.Nodes.Count)
                    .ToList();

                Logger.Log("Players", dragons.Select(dragon => new {dragon.Id, dragon.State}));
                var bestOpponent = otherPlayerInformation.First();
                Logger.Log("My distance", playerInformation.Nodes.Count);
                Logger.Log("Best opponent distance", bestOpponent.Nodes.Count);
                var action = "";
                if (bestOpponent.Nodes.Count <= playerInformation.Nodes.Count &&
                    playerInformation.Dragon.AvailableWalls > 0)
                {
                    Logger.Log("Trying to get wall to place.");
                    action = GetWall(bestOpponent, height, width, grid);
                    if (string.IsNullOrWhiteSpace(action))
                        action = GetMove(playerInformation, player);
                }
                else
                {
                    action = GetMove(playerInformation, player);
                }

                // Simply check if our count of nodes is less than that of the opponents, if it is not, set wall in front of
                // opponents with least amount of remaining nodes.

                // action: LEFT, RIGHT, UP, DOWN or "putX putY putOrientation" to place a wall
                Actions.Commit(action);

                round += 1;

                Logger.Log("Round", round);
            }
        }

        private static string GetMove(PlayerInformation playerInformation, Dragon player)
        {
            var firstNode = playerInformation.Nodes.FirstOrDefault();
            var action = player.TargetDirection.ToString("G").ToUpper();
            if (firstNode == null) return action;
            if (firstNode.X > player.X)
                action = "RIGHT";
            if (firstNode.X < player.X)
                action = "LEFT";
            if (firstNode.Y > player.Y)
                action = "DOWN";
            if (firstNode.Y < player.Y)
                action = "UP";

            return action;
        }

        private static string GetWall(PlayerInformation bestOpponent, int height, int width, Grid grid)
        {
            var action = "";
            // Some opponent is doing better than us. We need to wall them in.
            if (bestOpponent.Dragon.TargetDirection == TargetDirection.Right)
            {
                var wallX = bestOpponent.Dragon.X + 1;
                var wallY = bestOpponent.Dragon.Y;
                if (wallY == height) wallY -= 1;
                if (CanPlaceWall(wallX, wallY, WallDirection.Vertical, grid))
                    action = $"{wallX} {wallY} V";
            }

            if (bestOpponent.Dragon.TargetDirection == TargetDirection.Left)
            {
                var wallX = bestOpponent.Dragon.X;
                var wallY = bestOpponent.Dragon.Y;
                if (wallY == height) wallY -= 1;
                if (CanPlaceWall(wallX, wallY, WallDirection.Vertical, grid))
                    action = $"{wallX} {wallY} V";
            }

            if (bestOpponent.Dragon.TargetDirection == TargetDirection.Down)
            {
                var wallY = bestOpponent.Dragon.Y + 1;
                var wallX = bestOpponent.Dragon.X;
                if (wallX == width) wallX -= 1;
                if (CanPlaceWall(wallX, wallY, WallDirection.Vertical, grid))
                    action = $"{wallX} {wallY} H";
            }

            return action;
        }

        private static IEnumerable<GridNodeAStar> GetDragonTravelNodes(Dragon dragon, Grid grid)
        {
            var dragonTarget = dragon.TargetDirection;
            var target = dragonTarget switch
            {
                TargetDirection.Right => $"7-{dragon.Y}",
                TargetDirection.Left => $"0-{dragon.Y}",
                TargetDirection.Down => $"{dragon.X}-7",
                TargetDirection.Up => $"{dragon.X}-0"
            };
            var dragonCoordinates = $"{dragon.X}-{dragon.Y}";
            var travelNodes = grid.GetAStarNodesWithList(dragonCoordinates, target);
            return travelNodes;
        }

        private static bool CanPlaceWall(int x, int y, WallDirection wallDirection, Grid grid)
        {
            if (wallDirection == WallDirection.Vertical)
            {
                var upperLeftCellId = $"{x - 1}-{y}";
                var upperRightCellId = $"{x}-{y}";
                var lowerLeftCellId = $"{x - 1}-{y + 1}";
                var lowerRightCellId = $"{x}-{y + 1}";
                var upperConnectionExists = grid.DoesConnectionExist(upperLeftCellId, upperRightCellId);
                var lowerConnectionExists = grid.DoesConnectionExist(lowerLeftCellId, lowerRightCellId);
                return upperConnectionExists && lowerConnectionExists;
            }
            else
            {
                var upperLeftCellId = $"{x}-{y - 1}";
                var lowerLeftCellId = $"{x}-{y}";
                var upperRightCellId = $"{x + 1}-{y - 1}";
                var lowerRightCellId = $"{x + 1}-{y}";
                var leftConnectionExists = grid.DoesConnectionExist(upperLeftCellId, lowerLeftCellId);
                var rightConnectionExists = grid.DoesConnectionExist(upperRightCellId, lowerRightCellId);
                return leftConnectionExists && rightConnectionExists;
            }
        }
    }
}