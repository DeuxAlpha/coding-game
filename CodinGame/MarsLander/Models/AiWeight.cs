namespace CodinGame.MarsLander.Models
{
    public class AiWeight
    {
        public double HorizontalSpeedWeight { get; set; }
        public double VerticalSpeedWeight { get; set; }
        public double RotationWeight { get; set; }
        public double HorizontalDistanceWeight { get; set; }
        public double VerticalDistanceWeight { get; set; }
        public double FuelWeight { get; set; }
        public double BetterBias { get; set; }
        public double BetterCutoff { get; set; }
        public double MutationChance { get; set; }
        public double ElitismBias { get; set; }

        public AiWeight Clone()
        {
            return new AiWeight
            {
                HorizontalSpeedWeight = HorizontalSpeedWeight,
                VerticalSpeedWeight = VerticalSpeedWeight,
                RotationWeight = RotationWeight,
                HorizontalDistanceWeight = HorizontalDistanceWeight,
                BetterBias = BetterBias,
                BetterCutoff = BetterCutoff,
                MutationChance = MutationChance,
                ElitismBias = ElitismBias,
            };
        }

        public AiWeight()
        {
            HorizontalSpeedWeight = 1;
            VerticalSpeedWeight = 1;
            RotationWeight = 1;
            HorizontalDistanceWeight = 1;
            VerticalDistanceWeight = 1;
            FuelWeight = 1;
            BetterBias = 0.8;
            BetterCutoff = 0.2;
            MutationChance = 0.05;
            ElitismBias = 0.2;
        }
    }
}