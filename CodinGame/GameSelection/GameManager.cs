using System;
using CodinGame.GameOfDrones;
using CodinGame.GreatEscape;
using CodinGame.Interfaces;

namespace CodinGame.GameSelection
{
    public static class GameManager
    {
        public static void PlayGame(Game game)
        {
            switch (game)
            {
                case Game.GameOfDrones:
                    GameOfDronesManager.Play();
                    break;
                case Game.GreatEscape:
                    GreatEscapeManager.Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(game));
            }
        }
    }
}