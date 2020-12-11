using System.Collections.Generic;
using System.Linq;
using CodinGame.GameOfDrones.Models.Drones;
using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Models;

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
                        Trigonometry.GetDistance(
                            new Point(drone.Location.X, drone.Location.Y),
                            new Point(Center.X, Center.Y)) <
                        GameOfDronesManager.ZoneRadius));
        }
    }
}