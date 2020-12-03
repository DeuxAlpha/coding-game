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

        public Lander Clone()
        {
            var clone = new Lander
            {
                X = X,
                Y = Y,
                HorizontalSpeed = HorizontalSpeed,
                VerticalSpeed = VerticalSpeed,
                Fuel = Fuel,
                Rotation = Rotation,
                Power = Power
            };
            return clone;
        }

        public void Apply(int rotation, int power)
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