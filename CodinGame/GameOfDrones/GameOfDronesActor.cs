using System.Collections.Generic;
using System.Linq;
using CodinGame.GameOfDrones.Guessing;
using CodinGame.GameOfDrones.Models;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Maths;

namespace CodinGame.GameOfDrones
{
    public class GameOfDronesActor
    {
        public static void Act(int droneIndex)
        {
            var opponentDrones = GameOfDronesManager.Participants
                .Where(participant => !participant.IsPlayer)
                .Select(participant => new
                {
                    Drones = participant.Drones.Select(drone =>
                    {
                        var targetZone = TargetZoneGuesser.Guess(drone);
                        return new
                        {
                            DroneId = drone.Id,
                            TargetZone = targetZone,
                            TargetDistance = Trigonometry.GetDistance(drone.Location.X, drone.Location.Y,
                                targetZone.Center.X, targetZone.Center.Y),
                            ParticipantId = participant.Id
                        };
                    })
                })
                .SelectMany(drone => drone.Drones);

            var unoccupiedZones = GameOfDronesManager.Zones.Where(zone => zone.OwnerId == -1);
            var heldZones = GameOfDronesManager.Zones.Where(zone => zone.OwnerId == GameOfDronesManager.PlayerId)
                .Select(zone => new
                {
                    zone,
                    OpponentCount = GameOfDronesManager.Participants
                        .Select(participant => participant.Drones
                            .Count(drone =>
                                Trigonometry.GetDistance(
                                    drone.Location.X,
                                    drone.Location.Y,
                                    zone.Center.X,
                                    zone.Center.Y)
                                <= GameOfDronesManager.ZoneRadius))
                        .OrderByDescending(count => count)
                        .First()
                });

            // Find empty zones and send as many as is feasible (while keeping all currently held zones occupied).
            var untargetedZones = GameOfDronesManager.Zones.ToList();
            foreach (var targetZone in opponentDrones.Select(drone => drone.TargetZone))
            {
                untargetedZones.RemoveAll(zone => zone.Id == targetZone.Id);
            }

            // Assign player drones to target nearest unassigned zone.
            Actions.Commit(GoToNearestAvailableZone(droneIndex, untargetedZones));
        }

        private static string GoToNearestAvailableZone(
            int droneIndex,
            IEnumerable<Zone> availableZones)
        {
            var drone = GameOfDronesManager.Player.PlayerDrones[droneIndex];
            var zonesWithDistance = availableZones
                .Where(zone => zone.OwnerId != GameOfDronesManager.PlayerId)
                .Select(zone => new
            {
                zone.Center.X,
                zone.Center.Y,
                zone.OwnerId,
                DistanceToCurrentDrone =
                    Trigonometry.GetDistance(zone.Center.X, zone.Center.Y, drone.Location.X, drone.Location.Y)
            });
            var nearestZone = zonesWithDistance.OrderBy(zone => zone.DistanceToCurrentDrone).FirstOrDefault();
            if (nearestZone == null) return GoToNearestZone(droneIndex);
            drone.Target = $"{nearestZone.X} {nearestZone.Y}";
            return drone.Target;
        }

        private static string GoToNearestZone(int droneIndex)
        {
            var drone = GameOfDronesManager.Player.PlayerDrones[droneIndex];
            var zones = GameOfDronesManager.Zones
                .Where(zone => zone.OwnerId != GameOfDronesManager.PlayerId)
                .Select(zone => new
            {
                zone.Center.X,
                zone.Center.Y,
                zone.OwnerId,
                DistanceToCurrentDrone =
                    Trigonometry.GetDistance(zone.Center.X, zone.Center.Y, drone.Location.X, drone.Location.Y)
            });
            var nearestZone = zones
                .OrderBy(zone => zone.DistanceToCurrentDrone)
                .FirstOrDefault();
            if (nearestZone == null) return $"{drone.Location.X} {drone.Location.Y}";
            return $"{nearestZone.X} {nearestZone.Y}";
        }
    }
}