using System;
using CodinGame.GreatEscape.Models;
using CodinGame.Utilities.Game;

namespace CodinGame.GreatEscape
{
    public static class GreatEscapeActor
    {
        public static void GetToTheOtherSide()
        {
            var playerOrigin = GreatEscapeManager.Player.Origin;
            switch (playerOrigin)
            {
                case Origin.Top:
                    Actions.Commit("DOWN");
                    break;
                case Origin.Right:
                    Actions.Commit("LEFT");
                    break;
                case Origin.Bottom:
                    Actions.Commit("UP");
                    break;
                case Origin.Left:
                    Actions.Commit("RIGHT");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerOrigin));
            }
        }
    }
}