using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Console.Hand
{
    public class HandPlayer : IPlayHands 
    {
        private readonly IRenderCards _cardRenderer;

        public HandPlayer(IRenderCards cardRenderer)
        {
            _cardRenderer = cardRenderer;
        }

        public void PlayHand(IGameState gameState)
        {
            //play a hand
            //ignoring loners for now
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (gameState.CurrentPlayer.IsHuman)
                    {
                        var renderedCards = _cardRenderer.RenderCards(gameState.CurrentPlayer.Cards, new CardRenderingOptions
                        {
                            ShowIndexes = true
                        });

                        System.Console.WriteLine(renderedCards);

                        System.Console.WriteLine("It is your turn, which card would you like to play?");

                        //TODO: restrict cards to lead suit, etc
                        var keyPressed = char.ToUpperInvariant(System.Console.ReadKey(true).KeyChar);
                        
                        //TODO: restrict to range
                        var indexOfCard = int.Parse(keyPressed.ToString()) - 1;
                        var chosenCard = gameState.CurrentPlayer.Cards[indexOfCard];

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
                        var chosenCard = gameState.CurrentPlayer.ChooseCardToPlay(gameState);

                        System.Console.WriteLine($"{gameState.CurrentPlayer.Name} played the {chosenCard.Rank.Name} of {chosenCard.Suit.Name}:");
                        
                        gameState.CurrentHand.Add(new PlayedCard
                        {
                            Player = gameState.CurrentPlayer,
                            Card = chosenCard
                        });

                        gameState.CurrentPlayer.Cards = gameState.CurrentPlayer.Cards.ToList().Except(new List<ICard> {chosenCard}).ToList();
                    }

                    var renderedCard = _cardRenderer.RenderCards(gameState.CurrentHand.Select(x => x.Card).ToList(), new CardRenderingOptions
                    {
                        OrderByRanks = false
                    });

                    System.Console.WriteLine(renderedCard);

                    gameState.AdvanceToNextPlayer();
                }

                gameState.EvaluateHand();
                gameState.CurrentHand.Clear();
            }
        }
    }
}
