namespace CodinGame.GameOfDrones.Models.Drones
{
    public class PlayerDrone : Drone
    {
        public CurrentCommand Command { get; set; }

        public PlayerDrone(Drone drone)
        {
            Location = drone.Location;
            LocationHistory = drone.LocationHistory;
        }
    }
}