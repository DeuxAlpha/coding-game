using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.GameOfDrones.Models;
using CodinGame.GameOfDrones.Models.Drones;
using CodinGame.GameOfDrones.Models.Players;
using CodinGame.GameOfDrones.Models.Zones;
using CodinGame.Utilities.Game;

namespace CodinGame.GameOfDrones
{
    public static class GameOfDronesManager
    {
        /// <summary>Number of players in the game (2 to 4 players).</summary>
        public static int PlayerCount { get; private set; }
        /// <summary>ID of the Player (0, 1, 2, or 3).</summary>
        public static int PlayerId { get; private set; }
        /// <summary>Number of drones in each team (3 to 11).</summary>
        public static int DronesPerTeam { get; private set; }
        /// <summary>Number of zones on the map (4 to 8).</summary>
        public static int ZoneCount { get; private set; }
        public static List<Zone> Zones { get; private set; }
        public static List<Participant> Participants { get; private set; }
        public static Player Player { get; private set; }
        public static int Ticks { get; private set; }

        public const int ZoneRadius = 100;

        public static void Play()
        {
            var inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
            PlayerCount = int.Parse(inputs[0]); // number of players in the game (2 to 4 players)
            Participants = new List<Participant>();
            PlayerId = int.Parse(inputs[1]); // ID of your player (0, 1, 2, or 3)
            DronesPerTeam = int.Parse(inputs[2]); // number of drones in each team (3 to 11)
            for (var playerIndex = 0; playerIndex < PlayerCount; playerIndex++)
            {
                var participant = new Participant {Id = playerIndex};
                for (var droneIndex = 0; droneIndex < DronesPerTeam; droneIndex++)
                {
                    participant.Drones.Add(new Drone(droneIndex));
                }

                Participants.Add(participant);
            }

            Player = new Player(Participants.First(participant => participant.IsPlayer));
            ZoneCount = int.Parse(inputs[3]); // number of zones on the map (4 to 8)
            Zones = new List<Zone>();
            for (var i = 0; i < ZoneCount; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                // corresponds to the position of the center of a zone. A zone is a circle with a radius of 100 units.
                var x = int.Parse(inputs[0]);
                var y = int.Parse(inputs[1]);
                Zones.Add(new Zone
                {
                    Id = i,
                    Center = new Cell
                    {
                        X = x,
                        Y = y
                    },
                    OwnerId = -1
                });
            }

            // game loop
            while (true)
            {
                for (var i = 0; i < ZoneCount; i++)
                {
                    // ID of the team controlling the zone (0, 1, 2, or 3) or -1 if it is not controlled. The zones are given in the same order as in the initialization.
                    var ownerId = int.Parse(Console.ReadLine() ?? "-1");
                    Zones[i].OwnerId = ownerId;
                }

                for (var i = 0; i < PlayerCount; i++)
                {
                    for (var j = 0; j < DronesPerTeam; j++)
                    {
                        inputs = Console.ReadLine()?.Split(' ') ?? new string[] { };
                        // The first D lines contain the coordinates of drones of a player with the ID 0, the following D lines those of the drones of player 1, and thus it continues until the last player.
                        var droneX = int.Parse(inputs[0]);
                        var droneY = int.Parse(inputs[1]);
                        Participants[i].Drones[j].UpdateLocation(droneX, droneY);
                    }
                }

                for (var i = 0; i < DronesPerTeam; i++)
                {
                    GameOfDronesActor.Act(i);
                }

                Ticks += 1;
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}