using System.Linq;
using System;
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

            System.Console.WriteLine("Welcome to Euchre Time!");
            System.Console.WriteLine($"There are {gameState.Players.Where(x => x.IsHuman).Count()} human players and {gameState.Players.Where(x => !x.IsHuman).Count()} computer players.");
            
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
                    for (var j = 0; j < 4; j++)
                    {
                        if (gameState.CurrentPlayer.IsHuman)
                        {
                            //ask the human which card to play
                        }
                        else
                        {
                            gameState.CurrentPlayer.PlayCard(gameState);
                        }

                        gameState.AdvanceToNextPlayer();
                    }

                    gameState.EvaluateHand();
                }

                //advance the deal, reset state as-needed
                gameState.SetCurrentPlayerToLeftOfDealer();
                gameState.Dealer = gameState.CurrentPlayer;

                gameState.Trump = null;
                gameState.TurnedUpCard = null;
                gameState.OrderingUpPlayer = null;
                gameState.Kitty.Clear();
            }
        }

        private static void _askEachPlayerAboutTheTopCard(IGameState gameState)
        {
            System.Console.WriteLine("Beginning the bidding...");

            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                var shouldOrderUp = false;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    var keyPressed = ' ';

                    while (keyPressed != 'Y' && keyPressed != 'N' && keyPressed != 'y' && keyPressed != 'n')
                    {
                        System.Console.WriteLine($"Do you wish to order up {gameState.Dealer.Name} with the {gameState.TurnedUpCard.Rank.Name} of {gameState.TurnedUpCard.Suit.Name} (y/n)?");
                        keyPressed = System.Console.ReadKey(false).KeyChar;
                    }
                    
                    shouldOrderUp = keyPressed == 'Y' || keyPressed == 'y';

                    var action = shouldOrderUp ? "to order up trump" : "to pass";

                    System.Console.WriteLine($"{gameState.CurrentPlayer.Name} decided {action}.");
                }
                else
                {
                    shouldOrderUp = gameState.CurrentPlayer.PlayerStrategy.ShouldOrderUpDealerInFirstRound(gameState);

                    var action = shouldOrderUp ? "to order up trump" : "to pass";

                    System.Console.WriteLine($"{gameState.CurrentPlayer.Name} decided {action}.");
                }

                if (shouldOrderUp)
                {
                    //order up the dealer
                    gameState.Dealer.DiscardWhenOrderedUp(gameState);
                    gameState.Dealer.Cards.Add(gameState.TurnedUpCard);
                    gameState.Trump = gameState.TurnedUpCard.Suit;
                    gameState.OrderingUpPlayer = gameState.CurrentPlayer;
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

        private void _displayGameState(IGameState gameState)
        {
            System.Console.WriteLine("----------");
            System.Console.WriteLine();
            System.Console.WriteLine("----------");

        }
    }
}
