namespace CodinGame.GameOfDrones.Models.Drones
{
    public class PlayerDrone : Drone
    {
        public CurrentCommand Command { get; set; }
        public string Target { get; set; }

        public PlayerDrone(Drone drone) : base(drone.Id)
        {
            Location = drone.Location;
            LocationHistory = drone.LocationHistory;
        }
    }
}