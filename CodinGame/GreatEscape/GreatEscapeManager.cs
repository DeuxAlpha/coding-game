using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.GreatEscape.Models;
using CodinGame.Utilities.Game;

namespace CodinGame.GreatEscape
{
    public static class GreatEscapeManager
    {
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static int PlayerCount { get; private set; }
        public static int PlayerId { get; private set; }
        public static List<Participant> Participants { get; } = new List<Participant>();
        public static Participant Player => Participants.First(player => player.Id == PlayerId);
        public static int WallsOnBoard { get; private set; }
        public static List<Wall> Walls { get; private set; }
        public static int Ticks { get; private set; }

        public static void Play()
        {
            var inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
            Width = int.Parse(inputs[0]); // width of the board
            Height = int.Parse(inputs[1]); // height of the board
            PlayerCount = int.Parse(inputs[2]); // number of players (2 or 3)
            PlayerId = int.Parse(inputs[3]); // id of my player (0 = 1st player, 1 = 2nd player, ...)
            for (var playerIndex = 0; playerIndex < PlayerCount; playerIndex++)
            {
                Participants.Add(new Participant
                {
                    Id = playerIndex,
                });
            }

            var init = true;

            // game loop
            while (true)
            {
                for (var i = 0; i < PlayerCount; i++)
                {
                    inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                    var x = int.Parse(inputs[0]); // x-coordinate of the player
                    var y = int.Parse(inputs[1]); // y-coordinate of the player
                    var wallsLeft = int.Parse(inputs[2]); // number of walls available for the player
                    Participants[i].Update(x, y, wallsLeft);
                    if (init)
                        Participants[i].Initialize();
                }

                init = false;

                WallsOnBoard = int.Parse(Console.ReadLine() ?? ""); // number of walls on the board
                Walls = new List<Wall>();
                for (var i = 0; i < WallsOnBoard; i++)
                {
                    inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                    var wallX = int.Parse(inputs[0]); // x-coordinate of the wall
                    var wallY = int.Parse(inputs[1]); // y-coordinate of the wall
                    var wallOrientation = inputs[2]; // wall orientation ('H' or 'V')
                    Walls.Add(new Wall
                    {
                        Root = {X = wallX, Y = wallY},
                    });
                }

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");


                // action: LEFT, RIGHT, UP, DOWN or "putX putY putOrientation" to place a wall
                GreatEscapeActor.GetToTheOtherSide();

                Ticks += 1;
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}