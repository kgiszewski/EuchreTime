using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Suits;

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
                _askEachPlayerAboutTheTopCard(gameState);

                //if no takers, second round bidding
                if (gameState.Trump == null)
                {
                    _askEachPlayerAboutTrump(gameState);

                    //handle stick the dealer
                    if (gameState.Trump == null)
                    {
                    }
                }

                //return the current player to the left of the dealer
                gameState.SetCurrentPlayerToLeftOfDealer();

                //play a hand
                //ignoring loners for now
                for (var i = 0; i < 20; i++)
                {
                    gameState.AdvanceToNextPlayer();
                }


                //update score

                //advance the deal, reset state as-needed
                gameState.SetCurrentPlayerToLeftOfDealer();
                gameState.Dealer = gameState.CurrentPlayer;

                gameState.Trump = null;
                gameState.TurnedUpCard = null;
            }
        }

        private static void _askEachPlayerAboutTheTopCard(IGameState gameState)
        {
            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                var shouldOrderUp = false;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    //prompt the human for a yes/no
                    shouldOrderUp = false;
                }
                else
                {
                    shouldOrderUp = gameState.CurrentPlayer.PlayerStrategy.ShouldOrderUpDealerInFirstRound(gameState);
                }

                if (shouldOrderUp)
                {
                    //order up the dealer
                    break;
                }
            }
        }

        private static void _askEachPlayerAboutTrump(IGameState gameState)
        {
            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                ISuit trump = null;

                if(gameState.CurrentPlayer.IsHuman)
                {
                    //prompt the human for a yes/no
                    trump = null;
                }
                else
                {
                    trump = gameState.CurrentPlayer.PlayerStrategy.SecondRoundTrumpChoice(gameState);
                }

                if (trump != null)
                {
                    //set trump
                    break;
                }
            }
        }
    }
}
