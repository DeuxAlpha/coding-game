using System;
using System.Linq;
using CodinGame.GameOfDrones.Models;
using CodinGame.GameOfDrones.Models.Drones;
using CodinGame.GameOfDrones.Models.Zones;
using CodinGame.Utilities.Maths;
using CodinGame.Utilities.Maths.Models;

namespace CodinGame.GameOfDrones.Guessing
{
    public static class TargetZoneGuesser
    {
        public static Zone Guess(Drone drone)
        {
            // We need to check the angle the drone has been traveling on, and see if it's on the trajectory to a particular zone.
            var lastLocation = drone.LocationHistory.LastOrDefault();
            if (lastLocation == null) return null;
            var travelAngle = Trigonometry.GetAngle(new Point(lastLocation.X, lastLocation.Y),
                new Point(drone.Location.X, drone.Location.Y));
            return GameOfDronesManager.Zones
                .OrderBy(zone =>
                    // The amount the trajectory of the drone is off from getting to the zone.
                    Math.Abs(travelAngle - Trigonometry.GetAngle(new Point(drone.Location.X, drone.Location.Y),
                        new Point(zone.Center.X, zone.Center.Y))))
                .First();
        }
    }
}