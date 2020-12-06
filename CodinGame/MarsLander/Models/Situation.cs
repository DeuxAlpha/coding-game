namespace CodinGame.MarsLander.Models
{
    public class Situation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; set; }
        public int Fuel { get; set; }
        public int Power { get; set; }
        public int VerticalSpeed { get; set; }
        public int HorizontalSpeed { get; set; }

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