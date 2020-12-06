using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.Utilities.Game;
using CodinGame.Utilities.Maths;

namespace CodinGame.MarsLander.Models
{
    public class Lander
    {
        public Situation Situation { get; set; }
        public List<Situation> Situations { get; set; } = new List<Situation>();
        public List<string> Actions { get; private set; } = new List<string>();

        public Lander Clone()
        {
            var clone = new Lander
            {
                Situation = new Situation
                {
                    X = Situation.X,
                    Y = Situation.Y,
                    HorizontalSpeed = Situation.HorizontalSpeed,
                    VerticalSpeed = Situation.VerticalSpeed,
                    Fuel = Situation.Fuel,
                    Rotation = Situation.Rotation,
                    Power = Situation.Power,
                },
                Situations = Situations.ToList(),
                Actions = Actions.ToList()
            };
            return clone;
        }

        public void Apply(int rotation, int power)
        {
            if (Situation.Fuel < power) power = Situation.Fuel;
            Situation.Power = power;
            Situation.Rotation = rotation;
            Situation.Fuel -= power;
            Situation.HorizontalSpeed += (int) (Trigonometry.GetHorizontalSpeedFraction(Situation.Rotation) * Situation.Power);
            if (Situation.HorizontalSpeed > MarsLanderRules.MaxHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MaxHorizontalSpeed;
            if (Situation.HorizontalSpeed < MarsLanderRules.MinHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MinHorizontalSpeed;
            Situation.VerticalSpeed += (int) (Trigonometry.GetVerticalSpeedFraction(Situation.Rotation) * Situation.Power - MarsLanderRules.Gravity);
            if (Situation.VerticalSpeed > MarsLanderRules.MaxVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MaxVerticalSpeed;
            if (Situation.VerticalSpeed < MarsLanderRules.MinVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MinVerticalSpeed;
            Situation.X += Situation.HorizontalSpeed;
            Situation.Y += Situation.VerticalSpeed;
            Situations.Add(Situation.Clone());
            Actions.Add($"{rotation} {power}");
        }

        public string LimitMomentum()
        {
            var puppet = Clone();
            var currentAngle = Trigonometry.GetAngle(Situation.X, Situation.Y, Situation.X + Situation.HorizontalSpeed, Situation.Y + Situation.VerticalSpeed);
            var oppositeAngle = currentAngle - 270; // Right is -90 in game, Left is 90
            var currentSpeed = Trigonometry.GetDistance(Situation.X, Situation.Y, Situation.X + Situation.HorizontalSpeed, Situation.Y + Situation.VerticalSpeed);
            var speed = 0;
            if (oppositeAngle >= 0 && Situation.Rotation >= 0 || oppositeAngle <= 0 && Situation.Rotation <= 0)
                speed = 4;

            return $"{(int) Math.Round(oppositeAngle)} {speed}";
        }
    }
}