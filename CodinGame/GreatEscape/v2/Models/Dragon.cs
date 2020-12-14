using System.Collections;
using System.Collections.Generic;
using CodinGame.GreatEscape.v2.Models.Enums;

namespace CodinGame.GreatEscape.v2.Models
{
    public class Dragon
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int AvailableWalls { get; set; }
        public PlayerType PlayerType { get; }
        public TargetDirection TargetDirection { get; }
        public DragonState State { get; private set; }
        public int Id { get; }

        public Dragon(PlayerType playerType, int id)
        {
            PlayerType = playerType;
            Id = id;
            TargetDirection = GetTargetDirection();
            State = DragonState.Alive;
        }

        private TargetDirection GetTargetDirection()
        {
            return Id switch
            {
                0 => TargetDirection.Right,
                1 => TargetDirection.Left,
                2 => TargetDirection.Down,
                3 => TargetDirection.Up,
                _ => TargetDirection.Right
            };
        }

        public void Update(int x, int y, int availableWalls)
        {
            X = x;
            Y = y;
            AvailableWalls = availableWalls;
            if (X < 0 || Y < 0) State = DragonState.Dead;
        }
    }
}