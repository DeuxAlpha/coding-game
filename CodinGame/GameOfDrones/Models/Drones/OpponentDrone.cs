using System.Collections.Generic;
using System.Linq;
using CodinGame.GameOfDrones.Guessing;
using CodinGame.GameOfDrones.Models.Zones;
using CodinGame.Utilities.Maths;

namespace CodinGame.GameOfDrones.Models.Drones
{
    public class OpponentDrone : Drone
    {
        public Zone TargetZone { get; private set; }
        public double TargetDistance { get; private set; }
        public int OpponentId { get; private set; }

        public static IEnumerable<OpponentDrone> GetOpponentDrones()
        {
            return GameOfDronesManager.Participants
                .Where(participant => !participant.IsPlayer)
                .Select(participant => new
                {
                    Drones = participant.Drones.Select(drone =>
                    {
                        var targetZone = TargetZoneGuesser.Guess(drone);
                        return new OpponentDrone(drone.Id)
                        {
                            TargetZone = targetZone,
                            TargetDistance = Trigonometry.GetDistance(drone.Location.X, drone.Location.Y,
                                targetZone.Center.X, targetZone.Center.Y),
                            OpponentId = participant.Id
                        };
                    })
                })
                .SelectMany(drone => drone.Drones);
        }

        private OpponentDrone(int id) : base(id)
        {
        }
    }
}