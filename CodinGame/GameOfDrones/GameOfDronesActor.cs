using System.Linq;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Maths;

namespace CodinGame.GameOfDrones
{
    public class GameOfDronesActor
    {
        public void Act(int droneIndex)
        {
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            // output a destination point to be reached by one of your drones. The first line corresponds to the first of your drones that you were provided as input, the next to the second, etc.
            Actions.Commit(GoToNearestZone(droneIndex));
        }

        public string GoToNearestZone(int droneIndex)
        {
            var drone = GameOfDronesManager.Player.Drones[droneIndex];
            var zones = GameOfDronesManager.Zones.Select(zone => new
            {
                zone.Center.X,
                zone.Center.Y,
                zone.OwnerId,
                DistanceToCurrentDrone = Trigonometry.GetDistance(zone.Center.X, zone.Center.Y, drone.Location.X, drone.Location.Y)
            });
            Logger.Log("My Drone", drone);
            Logger.Log("Zones", zones);
            var nearestZone = zones
                .OrderBy(zone => zone.DistanceToCurrentDrone)
                .First();
            Logger.Log("Nearest Zone", nearestZone);
            return $"{nearestZone.X} {nearestZone.Y}";
        }
    }
}