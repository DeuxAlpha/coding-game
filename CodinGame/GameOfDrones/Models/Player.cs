using System.Collections.Generic;

namespace CodinGame.GameOfDrones.Models
{
    public class Player
    {
        public int Id { get; set; }
        public List<Drone> Drones { get; set; } = new List<Drone>();
        public bool IsPlayer => Id == GameOfDronesManager.PlayerId;
    }
}