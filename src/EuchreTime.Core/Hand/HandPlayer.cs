using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Extensions;
using EuchreTime.Core.Game;
using EuchreTime.Core.Helpers;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Hand
{
    public class HandPlayer : IPlayHands
    {
        public event AiPlayedCardHandler OnAiPlayedCard;
        public event TrickCompletedHandler OnTrickCompleted;
        public event HandCompletedHandler OnHandCompleted;

        public void PlayHand(IGameState gameState, Func<ICard> chooseHumanCard)
        {
            //play a hand
            //ignoring loners for now
            for (var i = 0; i < 5; i++)
            {
                //play a trick
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

                        OnAiPlayedCard?.Invoke(this, new AiPlayedCardEventArgs(chosenCard));

                        gameState.CurrentPlayer.Cards = gameState.CurrentPlayer.Cards.ToList().Except(new List<ICard> {chosenCard}).ToList();
                    }

                    if (gameState.LeadSuit == null)
                    {
                        if (chosenCard.IsTheLeft(gameState.Trump))
                        {
                            gameState.LeadSuit = CardHelper.GetOppositeSuit(chosenCard.Suit);
                        }
                        else
                        {
                            gameState.LeadSuit = chosenCard.Suit;
                        }
                    }

                    gameState.AdvanceToNextPlayer();
                }

                _evaluateTrick(gameState);
                gameState.CurrentHand.Clear();
                gameState.LeadSuit = null;
            }

            _evaluateHand(gameState);

            foreach (var player in gameState.Players)
            {
                player.TricksTaken = 0;
            }
        }

        public void _evaluateHand(IGameState gameState)
        {
            //find out which team got 3+ tricks
            var team1TrickCount = gameState.Players.Where(x => x.TeamNumber == 1).Sum(x => x.TricksTaken);
            var team2TrickCount = gameState.Players.Where(x => x.TeamNumber == 2).Sum(x => x.TricksTaken);

            var winningTeamNumber = (team1TrickCount > team2TrickCount) ? 1 : 2;
            var winningTeamTrickCount = (winningTeamNumber == 1) ? team1TrickCount : team2TrickCount;
            var scoreToAdd = 0;

            //if winning team ordered up
            if (gameState.OrderingUpPlayer.TeamNumber == winningTeamNumber)
            {
                if (winningTeamTrickCount == 5)
                {
                    scoreToAdd = 2;
                }
                else
                {
                    scoreToAdd = 1;
                }
            }
            else
            {
                //euchred!
                scoreToAdd = 2;
            }

            if (winningTeamNumber == 1)
            {
                gameState.TeamOneScore += scoreToAdd;
            }
            else
            {
                gameState.TeamTwoScore += scoreToAdd;
            }

            OnHandCompleted?.Invoke(this, new HandCompletedEventArgs(winningTeamNumber, scoreToAdd, winningTeamTrickCount, gameState.TeamOneScore, gameState.TeamTwoScore));
        }

        public void _evaluateTrick(IGameState gameState)
        {
            //determine the winner of the trick
            var trumpCardsPlayed =
                gameState.CurrentHand.Where(x => x.Card.Suit == gameState.Trump || x.Card.IsTheLeft(gameState.Trump)).ToList();

            IPlayer winner = null;

            if (trumpCardsPlayed.Any())
            {
                if (CardHelper.ContainsRight(gameState.Trump, trumpCardsPlayed.Select(x => x.Card).ToList()))
                {
                    winner = gameState.CurrentHand.First(x => x.Card.IsTheRight(gameState.Trump)).Player;
                }
                else if (CardHelper.ContainsLeft(gameState.Trump, trumpCardsPlayed.Select(x => x.Card).ToList()))
                {
                    winner = gameState.CurrentHand.First(x => x.Card.IsTheLeft(gameState.Trump)).Player;
                }
                else
                {
                    var highestTrumpPlayed = trumpCardsPlayed.OrderByDescending(x => x.Card.Rank.Value).First();
                    winner = gameState.CurrentHand.First(x => x.Card.Equals(highestTrumpPlayed.Card)).Player;
                }
            }
            else
            {
                //highest rank wins of lead suit
                var leadSuitCardsPlayed = gameState.CurrentHand.Where(x => x.Card.Suit == gameState.LeadSuit).ToList();

                var highestRankPlayed = leadSuitCardsPlayed.OrderByDescending(x => x.Card.Rank.Value).First();

                winner = gameState.CurrentHand.First(x => x.Card.Equals(highestRankPlayed.Card)).Player;
            }
            
            winner.TricksTaken++;

            gameState.CurrentPlayer = winner;

            OnTrickCompleted?.Invoke(this, new TrickCompletedEventArgs(winner));
        }
    }
}
