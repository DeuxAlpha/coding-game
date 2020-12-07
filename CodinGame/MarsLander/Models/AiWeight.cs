namespace CodinGame.MarsLander.Models
{
    public class AiWeight
    {
        public double DistanceFromSurfaceWeight { get; set; }
        public double HorizontalSpeedWeight { get; set; }
        public double VerticalSpeedWeight { get; set; }
        public double RotationWeight { get; set; }
        public double HorizontalDistanceWeight { get; set; }
        public double HorizontalCenterDistanceWeight { get; set; }
        public double BetterBias { get; set; }
        public double BetterCutoff { get; set; }
        public double MutationChance { get; set; }

        public AiWeight()
        {
            DistanceFromSurfaceWeight = 1;
            HorizontalSpeedWeight = 1;
            VerticalSpeedWeight = 1;
            RotationWeight = 1;
            HorizontalDistanceWeight = 1;
            HorizontalCenterDistanceWeight = 1;
            BetterBias = 0.8;
            BetterCutoff = 0.2;
            MutationChance = 0.05;
        }
    }
}