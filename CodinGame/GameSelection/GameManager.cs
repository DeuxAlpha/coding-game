using System;
using CodinGame.GameOfDrones;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(game));
            }
        }
    }
}