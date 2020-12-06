namespace CodinGame.MarsLander.Models
{
    public class AiWeight
    {
        public double DistanceFromSurfaceWeight { get; set; }
        public double HorizontalSpeedWeight { get; set; }
        public double VerticalSpeedWeight { get; set; }
        public double RotationWeight { get; set; }
        public double VerticalDistanceWeight { get; set; }
        public double VerticalDistanceToCenterWeight { get; set; }

        public AiWeight()
        {
            DistanceFromSurfaceWeight = 1;
            HorizontalSpeedWeight = 1;
            VerticalSpeedWeight = 1;
            RotationWeight = 1;
            VerticalDistanceWeight = 1;
            VerticalDistanceToCenterWeight = 1;
        }
    }
}