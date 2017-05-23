using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Console.Bidding
{
    public class FirstRoundBidder : IHandleFirstRoundBidding
    {
        private readonly IRenderCards _cardRenderer;

        public FirstRoundBidder(IRenderCards cardRenderer)
        {
            _cardRenderer = cardRenderer;
        }

        public void AskEachPlayerAboutTheTopCard(IGameState gameState)
        {
            System.Console.WriteLine("The following card was turned up:");

            var turnedUpRenderedCard = _cardRenderer.RenderCards(new List<ICard> { gameState.TurnedUpCard });

            System.Console.WriteLine(turnedUpRenderedCard);

            System.Console.WriteLine("Your cards are the following:");

            var humanPlayerCards = _cardRenderer.RenderCards(gameState.Dealer.Cards);

            System.Console.WriteLine(humanPlayerCards);

            System.Console.WriteLine("Beginning the bidding...");

            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                var shouldOrderUp = false;
                var keyPressed = ' ';

                if (gameState.CurrentPlayer.IsHuman)
                {
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
                    if (gameState.Dealer.IsHuman)
                    {
                        //ask which card to discard
                        System.Console.WriteLine("Enter the card number to discard (1, 2, 3, 4, 5):");

                        keyPressed = System.Console.ReadKey(false).KeyChar;

                        //TODO: range check
                        var index = Convert.ToInt32(keyPressed);

                        var cardToRemove = gameState.CurrentPlayer.Cards[index];
                        gameState.CurrentPlayer.Cards.RemoveAt(0);

                        gameState.Kitty.Push(cardToRemove);
                    }
                    else
                    {
                        gameState.Dealer.DiscardWhenOrderedUp(gameState);
                    }

                    gameState.Dealer.Cards.Add(gameState.TurnedUpCard);
                    gameState.Trump = gameState.TurnedUpCard.Suit;
                    gameState.OrderingUpPlayer = gameState.CurrentPlayer;
                    break;
                }
            }
        }
    }
}
