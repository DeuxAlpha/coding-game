namespace CodinGame.MarsLander.Models
{
    public class Situation
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Rotation { get; set; }
        public int Fuel { get; set; }
        public int Power { get; set; }
        public double VerticalSpeed { get; set; }
        public double HorizontalSpeed { get; set; }

        public Situation Clone()
        {
            return new Situation
            {
                X= X,
                Y= Y,
                Rotation = Rotation,
                Fuel = Fuel,
                Power = Power,
                VerticalSpeed = VerticalSpeed,
                HorizontalSpeed = HorizontalSpeed
            };
        }
    }
}