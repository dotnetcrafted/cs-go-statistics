using System;
using System.Threading;
using CSGSI;
using CSGSI.Nodes;

namespace GameStateIntegration
{
    static class Program
    {
        static GameStateListener _gsl;
        static void Main(string[] args)
        {
            _gsl = new GameStateListener(3000);
            _gsl.NewGameState += OnNewGameState;
            if (!_gsl.Start())
            {
                Console.WriteLine("here");
                Console.ReadKey();
                Environment.Exit(0);
            }
            
            var timer = new Timer(Callback, null, 0, 10000);
            
            Console.WriteLine("Listening...");
        }

        private static void Callback(object state)
        {
            var a = _gsl.CurrentGameState;
            var str = a != null ? a.JSON : "Failed";
            Console.WriteLine(str);
        }

        static void OnNewGameState(GameState gs)
        {
            if (gs.Round.Phase == RoundPhase.Live &&
                gs.Bomb.State == BombState.Planted &&
                gs.Previously.Bomb.State == BombState.Planting)
            {
                Console.WriteLine("Bomb has been planted.");
                Console.WriteLine(gs.JSON);
            }
        }
    }
}
