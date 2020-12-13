using System;
using System.Collections.Generic;
using CodinGame.GreatEscape.v2.Models;
using CodinGame.GreatEscape.v2.Models.Enums;

namespace CodinGame.GreatEscape.v2
{
    public class GreatEscapeManager
    {
        public static void Play()
        {
            var inputs = Console.ReadLine()!.Split(' ');
            var w = int.Parse(inputs[0]); // width of the board
            var h = int.Parse(inputs[1]); // height of the board
            var environment = new GreatEscapeEnvironment(h, w);
            var playerCount = int.Parse(inputs[2]); // number of players (2 or 3)
            var myId = int.Parse(inputs[3]); // id of my player (0 = 1st player, 1 = 2nd player, ...)
            var dragons = new List<Dragon>();
            for (var playerId = 0; playerId < playerCount; playerId++)
            {
                dragons.Add(new Dragon(playerId == myId ? PlayerType.Player : PlayerType.Opponent));
            }

            // game loop
            while (true)
            {
                for (var i = 0; i < playerCount; i++)
                {
                    inputs = Console.ReadLine()!.Split(' ');
                    var x = int.Parse(inputs[0]); // x-coordinate of the player
                    var y = int.Parse(inputs[1]); // y-coordinate of the player
                    var wallsLeft = int.Parse(inputs[2]); // number of walls available for the player
                    var cell = environment.Map.GetCell(x, y);
                    dragons[i].Update(cell, wallsLeft);
                }

                var wallCount = int.Parse(Console.ReadLine()!); // number of walls on the board
                for (var i = 0; i < wallCount; i++)
                {
                    inputs = Console.ReadLine()!.Split(' ');
                    var wallX = int.Parse(inputs[0]); // x-coordinate of the wall
                    var wallY = int.Parse(inputs[1]); // y-coordinate of the wall
                    var wallOrientation = inputs[2]; // wall orientation ('H' or 'V')
                    environment.UpdateWall(
                        wallX,
                        wallY,
                        wallOrientation == "H" ? WallDirection.Horizontal : WallDirection.Vertical);
                    environment.StoreHistory();
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // action: LEFT, RIGHT, UP, DOWN or "putX putY putOrientation" to place a wall
                Console.WriteLine("RIGHT");
            }
        }
    }
}