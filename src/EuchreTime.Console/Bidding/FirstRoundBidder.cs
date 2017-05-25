using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Console.Bidding
{
    public class FirstRoundBidder : IHandleFirstRoundBidding
    {
        private readonly IRenderCards _cardRenderer;
        private readonly IInputHelper _inputHelper;

        public FirstRoundBidder(
            IRenderCards cardRenderer,
            IInputHelper inputHelper
        )
        {
            _cardRenderer = cardRenderer;
            _inputHelper = inputHelper;
        }

        public void AskEachPlayerAboutTheTopCard(IGameState gameState)
        {
            System.Console.WriteLine("The following card was turned up:");

            var turnedUpRenderedCard = _cardRenderer.RenderCards(new List<ICard> { gameState.TurnedUpCard }, new CardRenderingOptions());

            System.Console.WriteLine(turnedUpRenderedCard);

            System.Console.WriteLine("Your cards are the following:");

            var humanPlayerCards = _cardRenderer.RenderCards(gameState.Dealer.Cards, new CardRenderingOptions
            {
                ShowIndexes = true
            });

            System.Console.WriteLine(humanPlayerCards);

            System.Console.WriteLine("Beginning the bidding...");

            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                var shouldOrderUp = false;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    var keyPressed =
                            _inputHelper.GetValidInput(
                                $"Do you wish to order up {gameState.Dealer.Name} with the {gameState.TurnedUpCard.Rank.Name} of {gameState.TurnedUpCard.Suit.Name}?",
                                new List<char> { 'y', 'n' }
                            );

                    shouldOrderUp = keyPressed == 'Y';

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
                        System.Console.WriteLine();

                        var keyPressed =
                            _inputHelper.GetValidInput(
                                "Enter the card number to discard:",
                                new List<char> { '1', '2', '3', '4', '5' }
                            );

                        var index = int.Parse(keyPressed.ToString());

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
