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
        private SurfaceZone _currentSurface;

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

        public void Apply(int rotationChange, int powerChange, MarsLanderEnvironment environment)
        {
            if (Status != LanderStatus.Flying) return;
            if (Situation.Fuel < Situation.Power + powerChange) powerChange = 0;
            if (Situation.Fuel < Situation.Power) powerChange = -1;
            Situation.Power += LimitPower(powerChange);
            Situation.Rotation += LimitRotation(rotationChange);
            Situation.Fuel -= Situation.Power;
            Situation.HorizontalSpeed += Trigonometry.GetHorizontalSpeedFraction(Situation.Rotation, ZeroDegreesDirection.Top) * Situation.Power;
            if (Situation.HorizontalSpeed > MarsLanderRules.MaxHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MaxHorizontalSpeed;
            if (Situation.HorizontalSpeed < MarsLanderRules.MinHorizontalSpeed)
                Situation.HorizontalSpeed = MarsLanderRules.MinHorizontalSpeed;
            Situation.VerticalSpeed += Trigonometry.GetVerticalSpeedFraction(Situation.Rotation, ZeroDegreesDirection.Top) * Situation.Power - MarsLanderRules.Gravity;
            if (Situation.VerticalSpeed > MarsLanderRules.MaxVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MaxVerticalSpeed;
            if (Situation.VerticalSpeed < MarsLanderRules.MinVerticalSpeed)
                Situation.VerticalSpeed = MarsLanderRules.MinVerticalSpeed;
            Situation.X += Situation.HorizontalSpeed;
            Situation.Y += Situation.VerticalSpeed;
            RoundValues();
            UpdateSurfaces(environment);
            Situations.Add(Situation.Clone());
            Actions.Add($"{rotationChange} {powerChange}");
            SetStatus(environment);
        }

        private void RoundValues()
        {
            Situation.HorizontalSpeed = Math.Round(Situation.HorizontalSpeed, 2);
            Situation.VerticalSpeed = Math.Round(Situation.VerticalSpeed, 2);
            Situation.X = Math.Round(Situation.X, 2);
            Situation.Y = Math.Round(Situation.Y, 2);
        }

        public bool WillHitLandingZone(MarsLanderEnvironment environment, int withinTheseTurns = 5)
        {
            var landingZone = environment.GetLandingZone();
            var puppet = Clone();
            var turns = 0;
            while (puppet.Status == LanderStatus.Flying && turns < withinTheseTurns)
            {
                puppet.Apply(0, 0, environment);
                turns += 1;
            }

            if (puppet.Status == LanderStatus.Flying) return false;
            return puppet.Situation.X >= landingZone.LeftX && puppet.Situation.X < landingZone.RightX;
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

            // TODO: We also need to create a quick function, then, to see if we crashed through a zone/surface element
            // Since we are not using the previous situation to actually calculate what we might have passed through
            // (We are only checking if we are under the surface (NOW), rather than if we have been at any point.
            // E.g. moving through something like this '/\' is entirely possible.
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
            if (_currentSurface?.LeftX <= Situation.X && _currentSurface?.RightX < Situation.X) return;
            // We might think that we just move left or right to the next surface elements, but there's a chance that
            // the lander skipped several zones at once, so we need to analyze the entire zone again.
            _currentSurface = environment.GetCurrentSurface(this);
        }

        private int LimitPower(int powerChange)
        {
            if (Situation.Power + powerChange > MarsLanderRules.MaxPower)
                powerChange = 0;
            if (Situation.Power + powerChange < MarsLanderRules.MinPower)
                powerChange = 0;
            return powerChange;
        }

        private int LimitRotation(int rotationChange)
        {
            if (Situation.Rotation + rotationChange > MarsLanderRules.MaxAngle)
                rotationChange = MarsLanderRules.MaxAngle - Situation.Rotation;
            if (Situation.Rotation + rotationChange < MarsLanderRules.MinAngle)
                rotationChange = MarsLanderRules.MinAngle - Situation.Rotation;
            return rotationChange;
        }
    }
}