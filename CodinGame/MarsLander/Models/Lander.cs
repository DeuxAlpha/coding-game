using System;
using System.Collections;
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
        private SurfaceZone _leftCurrentSurface;
        private SurfaceZone _rightCurrentSurface;

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
            Situation.Power = LimitPower(power);
            Situation.Rotation = LimitRotation(rotation);
            Situation.Fuel -= power;
            Situation.HorizontalSpeed +=
                (int) (Trigonometry.GetHorizontalSpeedFraction(Situation.Rotation, ZeroDegreesDirection.Top) *
                       Situation.Power);
            if (Situation.HorizontalSpeed > MarsLanderRules.MaxHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MaxHorizontalSpeed;
            if (Situation.HorizontalSpeed < MarsLanderRules.MinHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MinHorizontalSpeed;
            Situation.VerticalSpeed +=
                (int) (Trigonometry.GetVerticalSpeedFraction(Situation.Rotation, ZeroDegreesDirection.Top) *
                    Situation.Power - MarsLanderRules.Gravity);
            if (Situation.VerticalSpeed > MarsLanderRules.MaxVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MaxVerticalSpeed;
            if (Situation.VerticalSpeed < MarsLanderRules.MinVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MinVerticalSpeed;
            Situation.X += Situation.HorizontalSpeed;
            Situation.Y += Situation.VerticalSpeed;
            UpdateSurfaces(environment);
            Situations.Add(Situation.Clone());
            Actions.Add($"{Situation.Rotation} {Situation.Power}");
            SetStatus(environment);
        }

        public bool WillHitLandingZone(MarsLanderEnvironment environment, int withinTheseTurns = 5)
        {
            var landingZone = environment.GetLandingZone().ToList();
            var puppet = Clone();
            var turns = 0;
            while (puppet.Status == LanderStatus.Flying && turns < withinTheseTurns)
            {
                puppet.Apply(0, 0, environment);
                turns += 1;
            }

            if (puppet.Status == LanderStatus.Flying) return false;
            return puppet.Situation.X > landingZone.ElementAt(0).LeftX && puppet.Situation.X < landingZone.ElementAt(1).LeftX;
        }


        public string LimitMomentum()
        {
            var puppet = Clone();
            var currentAngle = Trigonometry.GetAngle(Situation.X, Situation.Y, Situation.X + Situation.HorizontalSpeed,
                Situation.Y + Situation.VerticalSpeed);
            var oppositeAngle = currentAngle - 270; // Right is -90 in game, Left is 90
            var currentSpeed = Trigonometry.GetDistance(Situation.X, Situation.Y,
                Situation.X + Situation.HorizontalSpeed, Situation.Y + Situation.VerticalSpeed);
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

            // TODO: Refactor surface elements to hold all surface zones, angles, and each element in memory.
            // Might help because we can access actual x and y coordinates via indices
            // Might be too eager though, so will require testing
            // TODO: We also need to create a quick function, then, to see if we crashed through a zone/surface element
            if (MarsLanderEnvironment.GetDistanceFromSurface(this, _leftCurrentSurface, _rightCurrentSurface) > 0)
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
                Math.Abs(Situation.VerticalSpeed) > MarsLanderRules.MaxVerticalLandingSpeed ||
                Math.Abs(Situation.HorizontalSpeed) > MarsLanderRules.MaxHorizontalLandingSpeed)
            {
                Status = LanderStatus.Crashed;
                return;
            }

            Status = LanderStatus.Landed;
        }

        private void UpdateSurfaces(MarsLanderEnvironment environment)
        {
            // Check if we even need to update the surfaces.
            if (_leftCurrentSurface?.LeftX < Situation.X && _rightCurrentSurface?.LeftX > Situation.X) return;
            // We might think that we just move left or right to the next surface elements, but there's a chance that
            // the lander skipped several zones at once, so we need to analyze the entire zone again.
            _leftCurrentSurface = environment.GetLeftCurrentSurface(this);
            _rightCurrentSurface = environment.GetRightCurrentSurface(this);
        }

        private int LimitPower(int power)
        {
            var powerDifference = power - Situation.Power;
            if (Math.Abs(powerDifference) > MarsLanderRules.MaxPowerChange)
                powerDifference = powerDifference > 0 ? MarsLanderRules.MaxPowerChange : -MarsLanderRules.MaxPowerChange;
            if (Situation.Power + powerDifference > MarsLanderRules.MaxPower)
                powerDifference = Situation.Power - MarsLanderRules.MaxPower;
            if (Situation.Power + powerDifference < MarsLanderRules.MinPower)
                powerDifference = MarsLanderRules.MinPower - Situation.Power;
            return Situation.Power + powerDifference;
        }

        private int LimitRotation(int rotation)
        {
            var rotationDifference = rotation - Situation.Rotation;
            if (Math.Abs(rotationDifference) > MarsLanderRules.MaxAngleChange)
                rotationDifference = rotationDifference > 0 ? MarsLanderRules.MaxAngleChange : -MarsLanderRules.MaxAngleChange;
            if (Situation.Rotation + rotationDifference > MarsLanderRules.MaxAngle)
                rotationDifference = Situation.Rotation - MarsLanderRules.MaxAngle;
            if (Situation.Rotation + rotationDifference < MarsLanderRules.MinAngle)
                rotationDifference = MarsLanderRules.MinAngle - Situation.Rotation;
            return Situation.Rotation + rotationDifference;
        }
    }
}