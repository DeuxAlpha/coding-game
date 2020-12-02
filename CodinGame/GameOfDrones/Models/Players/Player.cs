using System.Collections.Generic;
using CodinGame.GameOfDrones.Models.Drones;

namespace CodinGame.GameOfDrones.Models.Players
{
    public class Player : Participant
    {
        public List<PlayerDrone> PlayerDrones { get; } = new List<PlayerDrone>();

        public Player(Participant participant)
        {
            Id = participant.Id;
            Drones = participant.Drones;
            foreach (var drone in participant.Drones)
            {
                PlayerDrones.Add(new PlayerDrone(drone));
            }
        }
    }
}