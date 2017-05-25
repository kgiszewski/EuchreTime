using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Hand
{
    public class HandPlayer : IPlayHands
    { 
        public void PlayHand(IGameState gameState, Func<ICard> chooseHumanCard, Action<ICard> aiChoseCardCallback)
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
                        chosenCard = chooseHumanCard();
                        
                        gameState.CurrentHand.Add(new PlayedCard
                        {
                            Player = gameState.CurrentPlayer,
                            Card = chosenCard
                        });

                        gameState.CurrentPlayer.Cards.Remove(chosenCard);
                    }
                    else
                    {
                        chosenCard = gameState.CurrentPlayer.ChooseCardToPlay(gameState);

                        gameState.CurrentHand.Add(new PlayedCard
                        {
                            Player = gameState.CurrentPlayer,
                            Card = chosenCard
                        });

                        //TODO: consider observable instead
                        aiChoseCardCallback(chosenCard);

                        gameState.CurrentPlayer.Cards = gameState.CurrentPlayer.Cards.ToList().Except(new List<ICard> {chosenCard}).ToList();
                    }

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
