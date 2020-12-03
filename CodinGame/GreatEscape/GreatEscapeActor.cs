using System;
using System.Linq;
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
                case Side.Up:
                    Actions.Commit("DOWN");
                    break;
                case Side.Right:
                    Actions.Commit("LEFT");
                    break;
                case Side.Down:
                    Actions.Commit("UP");
                    break;
                case Side.Left:
                    Actions.Commit("RIGHT");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerOrigin));
            }
        }

        /// <summary>Simple method that checks if we are first. If it is, then just go to the other side (since
        /// opponents generally don't set walls yet). If we are not first, lock opponent in when the chance arises (i.e.
        /// make them go a complex path. Then, proceed to exit.</summary>
        public static void Win()
        {
            if (GreatEscapeManager.Player.Id == 0)
                Actions.Commit(GreatEscapeManager.Player.Exit);
            else
            {
                if (GreatEscapeManager.Ticks < 1)
                    Actions.Commit(GreatEscapeManager.Player.Exit);

                // For now, only focus on one opponent.
                var opponent = GreatEscapeManager.Participants
                    .First(participant => participant.Id != GreatEscapeManager.Player.Id);

                var opponentDestination = opponent.Destination;
            }
        }
    }
}