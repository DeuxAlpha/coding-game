using System.Linq;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Maths;

namespace CodinGame.GameOfDrones
{
    public class GameOfDronesActor
    {
        public void Act(int droneIndex)
        {
            // Go to nearest zone. Then analyse board and see easy additional points (empty zones).
            // In bronze, nobody does a lot of logic in regards to resource management.
            if (GameOfDronesManager.Turns < 10)
                Actions.Commit(GoToNearestZone(droneIndex));
            else
            {
                // Find empty zones and send as many as is feasible (while keeping all currently held zones occupied).
                Logger.Log("Player", GameOfDronesManager.Player);
            }
        }

        private static string GoToNearestZone(int droneIndex)
        {
            var drone = GameOfDronesManager.Player.PlayerDrones[droneIndex];
            var zones = GameOfDronesManager.Zones.Select(zone => new
            {
                zone.Center.X,
                zone.Center.Y,
                zone.OwnerId,
                DistanceToCurrentDrone = Trigonometry.GetDistance(zone.Center.X, zone.Center.Y, drone.Location.X, drone.Location.Y)
            });
            var nearestZone = zones
                .OrderBy(zone => zone.DistanceToCurrentDrone)
                .First();
            return $"{nearestZone.X} {nearestZone.Y}";
        }
    }
}