using System;
using System.Linq;
using CodinGame.GameOfDrones.Models;
using CodinGame.GameOfDrones.Models.Drones;
using CodinGame.GameOfDrones.Models.Zones;
using CodinGame.Utilities.Maths;

namespace CodinGame.GameOfDrones.Guessing
{
    public static class TargetZoneGuesser
    {
        public static Zone Guess(Drone drone)
        {
            // We need to check the angle the drone has been traveling on, and see if it's on the trajectory to a particular zone.
            var lastLocation = drone.LocationHistory.LastOrDefault();
            if (lastLocation == null) return null;
            var travelAngle = Trigonometry.GetAngle(lastLocation.X, lastLocation.Y, drone.Location.X, drone.Location.Y);
            return GameOfDronesManager.Zones
                .OrderBy(zone =>
                    // The amount the trajectory of the drone is off from getting to the zone.
                    Math.Abs(travelAngle - Trigonometry.GetAngle(drone.Location.X, drone.Location.Y, zone.Center.X,zone.Center.Y)))
                .First();
        }
    }
}