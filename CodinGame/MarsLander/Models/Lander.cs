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
        public LanderStatus Status { get; private set; }

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
                Actions = Actions.ToList(),
                Status = Status
            };
            return clone;
        }

        public void Apply(int rotation, int power, MarsLanderEnvironment environment)
        {
            if (Status != LanderStatus.Flying) return;
            if (Situation.Fuel < power) power = Situation.Fuel;
            Situation.Power = power;
            Situation.Rotation = rotation;
            Situation.Fuel -= power;
            Situation.HorizontalSpeed += (int) (Trigonometry.GetHorizontalSpeedFraction(Situation.Rotation, ZeroDegreesDirection.Top) * Situation.Power);
            if (Situation.HorizontalSpeed > MarsLanderRules.MaxHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MaxHorizontalSpeed;
            if (Situation.HorizontalSpeed < MarsLanderRules.MinHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MinHorizontalSpeed;
            Situation.VerticalSpeed += (int) (Trigonometry.GetVerticalSpeedFraction(Situation.Rotation, ZeroDegreesDirection.Top) * Situation.Power - MarsLanderRules.Gravity);
            if (Situation.VerticalSpeed > MarsLanderRules.MaxVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MaxVerticalSpeed;
            if (Situation.VerticalSpeed < MarsLanderRules.MinVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MinVerticalSpeed;
            Situation.X += Situation.HorizontalSpeed;
            Situation.Y += Situation.VerticalSpeed;
            Situations.Add(Situation.Clone());
            Actions.Add($"{rotation} {power}");
            SetStatus(environment);
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

        private void SetStatus(MarsLanderEnvironment environment)
        {
            if (Status != LanderStatus.Flying) return;
            var previousSituation = Situations.LastOrDefault();
            if (previousSituation == null)
            {
                Status = LanderStatus.Flying;
                return;
            }

            if (MarsLanderEnvironment.IsLanderLost(this))
            {
                Status = LanderStatus.Lost;
                return;
            }

            if (environment.GetDistanceFromSurface(this) > 0)
            {
                Status = LanderStatus.Flying;
                return;
            }

            // We just moved through the ground. So analyze speed and rotation and see if we landed or crashed.
            // But first, make sure the ground is flat.
            if (environment.GetDistanceFromFlatSurface(this).HorizontalDistance > 0)
            {
                Status = LanderStatus.Crashed;
                return;
            }

            if (Situation.Rotation > MarsLanderRules.MaxLandingAngle ||
                Situation.VerticalSpeed > MarsLanderRules.MaxVerticalLandingSpeed ||
                Situation.HorizontalSpeed > MarsLanderRules.MaxHorizontalLandingSpeed)
            {
                Status = LanderStatus.Crashed;
                return;
            }

            Status = LanderStatus.Landed;
        }
    }
}