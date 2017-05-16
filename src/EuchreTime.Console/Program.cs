using EuchreTime.Core.Game;

namespace EuchreTime.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //init
            var gameState = new GameState();

            //game loop here
            while (!gameState.HasAnyTeamWon())
            {
                gameState.GetDealer().Deal(gameState);
            }
        }
    }
}
