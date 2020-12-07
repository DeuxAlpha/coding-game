namespace CodinGame.Utilities.Maths
{
    public class Thrust
    {
        public double Rotation { get; set; }
        public double Force { get; set; }

        public Thrust(double rotation, double force)
        {
            Rotation = rotation;
            Force = force;
        }

        public Thrust()
        {

        }
    }
}