namespace CodinGame.Utilities.Maths
{
    public class ThrustRequest
    {
        public double CurrentX { get; set; }
        public double CurrentY { get; set; }
        public double CurrentXSpeed { get; set; }
        public double CurrentYSpeed { get; set; }
        public double CurrentRotation { get; set; }
        public double DesiredX { get; set; }
        public double DesiredY { get; set; }
        public double DesiredXSpeed { get; set; }
        public double DesiredYSpeed { get; set; }
        public double DesiredRotation { get; set; }
        public double MaxRotationChange { get; set; } = double.MaxValue;
        public double MaxPowerChange { get; set; } = double.MaxValue;
        public double MaxRotation { get; set; } = double.MaxValue;
        public double MaxPower { get; set; } = double.MaxValue;
        public double MaxXSpeed { get; set; } = double.MaxValue;
        public double MaxYSpeed { get; set; } = double.MaxValue;
        public double Gravity { get; set; } = 0;

        public double XTolerance { get; set; } = 0.5;
        public double YTolerance { get; set; } = 0.5;
        public double XSpeedTolerance { get; set; } = 1;
        public double YSpeedTolerance { get; set; } = 1;
        public double RotationTolerance { get; set; } = 0;
    }
}