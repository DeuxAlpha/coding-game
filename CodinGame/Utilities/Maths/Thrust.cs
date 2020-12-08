namespace CodinGame.Utilities.Maths
{
    public class Thrust
    {
        public double Rotation { get; set; }
        public double Power { get; set; }

        public Thrust(double rotation, double power)
        {
            Rotation = rotation;
            Power = power;
        }

        public Thrust()
        {

        }
    }
}