using System.Collections.Generic;
using CodinGame.GameOfDrones.Models.Drones;

namespace CodinGame.GameOfDrones.Models.Players
{
    public class Participant
    {
        public int Id { get; set; }
        public List<Drone> Drones { get; protected set; } = new List<Drone>();
        public bool IsPlayer => Id == GameOfDronesManager.PlayerId;
    }
}