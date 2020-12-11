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

        public string GetApplicableAction(string desiredAction)
        {
            var actionArray = desiredAction.Split(" ");
            var desiredRotation = int.Parse(actionArray[0]);
            var desiredPower = int.Parse(actionArray[1]);
            if (Math.Abs(Situation.Rotation - desiredRotation) > MarsLanderRules.MaxAngleChange)
            {
                if (desiredRotation > Situation.Rotation)
                    desiredRotation = Situation.Rotation + MarsLanderRules.MaxAngleChange;
                else
                    desiredRotation = Situation.Rotation - MarsLanderRules.MaxAngleChange;
            }

            if (Math.Abs(Situation.Power - desiredPower) > MarsLanderRules.MaxPowerChange)
            {
                if (desiredPower > Situation.Power)
                    desiredPower = Situation.Power + MarsLanderRules.MaxPowerChange;
                else
                    desiredPower = Situation.Power - MarsLanderRules.MaxPowerChange;
            }

            return $"{desiredRotation} {desiredPower}";
        }

        public void Apply(string action, MarsLanderEnvironment environment)
        {
            var actionArray = action.Split(" ");
            Apply(int.Parse(actionArray[0]), int.Parse(actionArray[1]), environment);
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
            var puppet = Clone();
            var landingZone = environment.GetLandingZone();
            var turns = 0;
            while (puppet.Status == LanderStatus.Flying && turns < withinTheseTurns)
            {
                puppet.Apply(0, 0, environment);
                turns += 1;
            }

            if (puppet.Status == LanderStatus.Flying) return false;
            return puppet.Situation.X >= landingZone.LeftX && puppet.Situation.X < landingZone.RightX;
        }


        private void SetStatus(MarsLanderEnvironment environment)
        {
            if (Status != LanderStatus.Flying) return;

            if (MarsLanderEnvironment.IsLanderLost(this))
            {
                Status = LanderStatus.Lost;
                return;
            }

            // TODO: We set Landed in here even though in CodinGame we crash.
            if (!environment.LanderCrashedThroughGround(this))
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

            if (Math.Abs(Situation.Rotation) > MarsLanderRules.MaxLandingAngle ||
                Math.Abs(Situation.VerticalSpeed) > MarsLanderRules.MaxVerticalLandingSpeed ||
                Math.Abs(Situation.HorizontalSpeed) > MarsLanderRules.MaxHorizontalLandingSpeed)
            {
                Status = LanderStatus.Crashed;
                return;
            }

            Status = LanderStatus.Landed;
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