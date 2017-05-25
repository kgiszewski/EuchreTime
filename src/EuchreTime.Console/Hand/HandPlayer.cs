using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using EuchreTime.Core.Helpers;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Hand
{
    public class HandPlayer : IPlayHands 
    {
        private readonly IRenderCards _cardRenderer;
        private readonly IInputHelper _inputHelper;
        private readonly ICardHelper _cardHelper;

        public HandPlayer(
            IRenderCards cardRenderer,
            IInputHelper inputHelper,
            ICardHelper cardHelper
        )
        {
            _cardRenderer = cardRenderer;
            _inputHelper = inputHelper;
            _cardHelper = cardHelper;
        }

        public void PlayHand(IGameState gameState)
        {
            //play a hand
            //ignoring loners for now
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    ICard chosenCard = null;

                    if (gameState.CurrentPlayer.IsHuman)
                    {
                        var renderedCards = _cardRenderer.RenderCards(gameState.CurrentPlayer.Cards, new CardRenderingOptions
                        {
                            ShowIndexes = true
                        });

                        System.Console.WriteLine(renderedCards);

                        var keyPressed =
                            _inputHelper.GetValidInput(
                                "It is your turn, which card would you like to play?",
                                _cardHelper.GetValidIndexes(gameState.LeadSuit, gameState.CurrentPlayer.Cards)
                            );

                        var indexOfCard = int.Parse(keyPressed.ToString()) - 1;
                        chosenCard = gameState.CurrentPlayer.Cards[indexOfCard];

                        System.Console.WriteLine($"You played the {chosenCard.Rank.Name} of {chosenCard.Suit.Name}:");

                        gameState.CurrentHand.Add(new PlayedCard
                        {
                            Player = gameState.CurrentPlayer,
                            Card = chosenCard
                        });

                        gameState.CurrentPlayer.Cards.RemoveAt(indexOfCard);
                    }
                    else
                    {
                        chosenCard = gameState.CurrentPlayer.ChooseCardToPlay(gameState);

                        System.Console.WriteLine($"{gameState.CurrentPlayer.Name} played the {chosenCard.Rank.Name} of {chosenCard.Suit.Name}:");
                        
                        gameState.CurrentHand.Add(new PlayedCard
                        {
                            Player = gameState.CurrentPlayer,
                            Card = chosenCard
                        });

                        gameState.CurrentPlayer.Cards = gameState.CurrentPlayer.Cards.ToList().Except(new List<ICard> {chosenCard}).ToList();
                    }

                    var renderedCard = _cardRenderer.RenderCards(gameState.CurrentHand.Select(x => x.Card).ToList(), new CardRenderingOptions());

                    System.Console.WriteLine(renderedCard);

                    if (gameState.LeadSuit == null)
                    {
                        gameState.LeadSuit = chosenCard.Suit;
                    }

                    gameState.AdvanceToNextPlayer();
                }

                gameState.EvaluateHand();
                gameState.CurrentHand.Clear();
            }
        }
    }
}
