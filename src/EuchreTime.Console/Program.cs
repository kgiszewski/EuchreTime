using EuchreTime.Core.Game;

namespace EuchreTime.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //init
            var gameState = new GameState();

            //main game loop here
            while (!gameState.WinningConditions.HasAnyTeamWon(gameState))
            {
                gameState.Dealer.Deal(gameState);

                //first round bid
                //if no takers, second round bid
                ////handle stick the dealer above
                
                //play a hand

                //advance the deal
            }
        }
    }
}
