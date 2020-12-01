using System;

namespace CodinGame.GameOfDrones
{
    public static class GameOfDronesManager
    {
        public static int PlayerCount { get; private set; }
        public static int PlayerId { get; private set; }
        public static int DronesPerTeam { get; private set; }
        public static int ZoneCount { get; private set; }

        public static void Play()
        {
            var inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
            PlayerCount = int.Parse(inputs[0]); // number of players in the game (2 to 4 players)
            PlayerId = int.Parse(inputs[1]); // ID of your player (0, 1, 2, or 3)
            DronesPerTeam = int.Parse(inputs[2]); // number of drones in each team (3 to 11)
            ZoneCount = int.Parse(inputs[3]); // number of zones on the map (4 to 8)
            for (var i = 0; i < ZoneCount; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                // corresponds to the position of the center of a zone. A zone is a circle with a radius of 100 units.
                var x = int.Parse(inputs[0]);
                var y = int.Parse(inputs[1]);
            }

            // game loop
            while (true)
            {
                for (var i = 0; i < ZoneCount; i++)
                {
                    // ID of the team controlling the zone (0, 1, 2, or 3) or -1 if it is not controlled. The zones are given in the same order as in the initialization.
                    var ownerId = int.Parse(Console.ReadLine() ?? "-1");
                }

                for (var i = 0; i < PlayerCount; i++)
                {
                    for (var j = 0; j < DronesPerTeam; j++)
                    {
                        inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                        // The first D lines contain the coordinates of drones of a player with the ID 0, the following D lines those of the drones of player 1, and thus it continues until the last player.
                        var droneX = int.Parse(inputs[0]);
                        var droneY = int.Parse(inputs[1]);
                    }
                }

                for (var i = 0; i < DronesPerTeam; i++)
                {
                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");

                    // output a destination point to be reached by one of your drones. The first line corresponds to the first of your drones that you were provided as input, the next to the second, etc.
                    Console.WriteLine("20 20");
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}