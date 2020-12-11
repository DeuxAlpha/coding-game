using System.Collections.Generic;
using System.Linq;
using CodinGame.GameOfDrones.Guessing;
using CodinGame.GameOfDrones.Models;
using CodinGame.GameOfDrones.Models.Drones;
using CodinGame.GameOfDrones.Models.Zones;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Models;

namespace CodinGame.GameOfDrones
{
    public class GameOfDronesActor
    {
        public static void Act(int droneIndex)
        {
            var opponentDrones = OpponentDrone.GetOpponentDrones();

            var unoccupiedZones = GameOfDronesManager.Zones.Where(zone => zone.OwnerId == -1);
            var heldZones = GameOfDronesManager.Zones.Where(zone => zone.OwnerId == GameOfDronesManager.PlayerId)
                .Select(zone => new
                {
                    zone,
                    OpponentCount = GameOfDronesManager.Participants
                        .Select(participant => participant.Drones
                            .Count(drone =>
                                Trigonometry.GetDistance(
                                    new Point(drone.Location.X, drone.Location.Y),
                                    new Point(zone.Center.X, zone.Center.Y))
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
                        Trigonometry.GetDistance(
                            new Point(zone.Center.X, zone.Center.Y),
                            new Point(drone.Location.X, drone.Location.Y))
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
                        Trigonometry.GetDistance(
                            new Point(zone.Center.X, zone.Center.Y),
                            new Point(drone.Location.X, drone.Location.Y))
                });
            var nearestZone = zones
                .OrderBy(zone => zone.DistanceToCurrentDrone)
                .FirstOrDefault();
            if (nearestZone == null) return $"{drone.Location.X} {drone.Location.Y}";
            return $"{nearestZone.X} {nearestZone.Y}";
        }
    }
}