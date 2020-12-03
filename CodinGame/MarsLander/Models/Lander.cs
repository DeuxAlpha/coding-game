using System;
using CodinGame.Utilities.Maths;

namespace CodinGame.MarsLander.Models
{
    public class Lander
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int HorizontalSpeed { get; set; }
        public int VerticalSpeed { get; set; }
        public int Fuel { get; set; }
        public int Rotation { get; set; }
        public int Power { get; set; }

        public static Lander Clone(Lander original)
        {
            var clone = new Lander
            {
                X = original.X,
                Y = original.Y,
                HorizontalSpeed = original.HorizontalSpeed,
                VerticalSpeed = original.VerticalSpeed,
                Fuel = original.Fuel,
                Rotation = original.Rotation,
                Power = original.Power
            };
            return clone;
        }

        public void Apply(int power, int rotation)
        {
            if (Fuel < power) power = Fuel;
            Power = power;
            Rotation = rotation;
            Fuel -= power;
            HorizontalSpeed += (int) (Trigonometry.GetHorizontalSpeedFraction(Rotation) * Power);
            if (HorizontalSpeed > MarsLanderRules.MaxHorizontalSpeed)
                HorizontalSpeed = MarsLanderRules.MaxHorizontalSpeed;
            if (HorizontalSpeed < MarsLanderRules.MinHorizontalSpeed)
                HorizontalSpeed = MarsLanderRules.MinHorizontalSpeed;
            VerticalSpeed += (int) (Trigonometry.GetVerticalSpeedFraction(Rotation) * Power - MarsLanderRules.Gravity);
            if (VerticalSpeed > MarsLanderRules.MaxVerticalSpeed)
                VerticalSpeed = MarsLanderRules.MaxVerticalSpeed;
            if (VerticalSpeed < MarsLanderRules.MinVerticalSpeed)
                VerticalSpeed = MarsLanderRules.MinVerticalSpeed;
            X += HorizontalSpeed;
            Y += VerticalSpeed;
        }
    }
}