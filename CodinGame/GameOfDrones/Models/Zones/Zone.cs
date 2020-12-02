using System.Collections.Generic;
using System.Linq;
using CodinGame.GameOfDrones.Models.Drones;
using CodinGame.Utilities.Maths;

namespace CodinGame.GameOfDrones.Models.Zones
{
    /// <summary>A zone is a circle with a radius of 100 units.</summary>
    public class Zone
    {
        public int Id { get; set; }
        public Cell Center { get; set; }
        public int OwnerId { get; set; }

        public IEnumerable<Drone> GetOccupyingDrones()
        {
            return GameOfDronesManager.Participants
                .SelectMany(participant => participant.Drones
                    .Where(drone =>
                        Trigonometry.GetDistance(drone.Location.X, drone.Location.Y, Center.X, Center.Y) <
                        GameOfDronesManager.ZoneRadius));
        }
    }
}