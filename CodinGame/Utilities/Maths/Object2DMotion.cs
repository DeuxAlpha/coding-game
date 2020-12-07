namespace CodinGame.Utilities.Maths
{
    public class Object2DMotion
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double XSpeed { get; set; }
        public double YSpeed { get; set; }
        public double Rotation { get; set; }

        public Object2DMotion Clone()
        {
            return new Object2DMotion
            {
                X =X,
                Y= Y,
                XSpeed = XSpeed,
                YSpeed = YSpeed,
                Rotation = Rotation
            };
        }
    }
}