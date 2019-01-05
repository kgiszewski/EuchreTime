using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Extensions;
using EuchreTime.Core.Game;
using EuchreTime.Core.Helpers;
using MechanicGrip.Cards;
using MechanicGrip.Ranks;
using MechanicGrip.Suits;

namespace EuchreTime.Core.Players.PlayerStrategies
{
    public abstract class PlayerStrategyBase : IPlayerStrategy
    {
        public virtual bool ShouldOrderUpDealerInFirstRound(IGameState gameState)
        {
            //if I'm dealing, consider picking up based on a total number of trump I would have
            var myTrump = CardHelper.GetAllTrump(gameState.TurnedUpCard.Suit, gameState.CurrentPlayer.Cards);

            var trumpCount = myTrump.Count + (gameState.CurrentPlayer.IsDealing(gameState) ? 1 : 0);

            if (trumpCount > 2)
            {
                //if a bower is turned up, pass if our team is not dealing
                if (gameState.TurnedUpCard.Rank.Symbol == StandardRank.Jack &&
                    (!gameState.CurrentPlayer.IsDealing(gameState) ||
                     !gameState.CurrentPlayer.Partner(gameState).IsDealing(gameState)))
                {
                    return false;
                }

                //we have at least 3 trump and we're not ordering up a bower
                return true;
            }

            return false;
        }

        public virtual ISuit SecondRoundTrumpChoice(IGameState gameState)
        {
            //if I have at least 3 trump of any suit, order up if the sum of the ranks over a certain amt
            var validSuits = CardHelper.GetSuitsToChooseFrom(gameState.TurnedUpCard.Suit);

            var rankTotal = 0;
            ISuit suitToOrderUp = null;

            foreach (var suit in validSuits)
            {
                var suitRankTotal = gameState.CurrentPlayer.Cards.Where(x => x.Suit == suit).Sum(x => x.Rank.Value);
                var thisSuit = suit;

                //provide a boost for any bowers
                if (CardHelper.ContainsLeft(suit, gameState.CurrentPlayer.Cards))
                {
                    suitRankTotal += 4;
                }

                if (CardHelper.ContainsRight(suit, gameState.CurrentPlayer.Cards))
                {
                    suitRankTotal += 5;
                }

                if (suitRankTotal > 43 && suitRankTotal > rankTotal)
                {
                    rankTotal = suitRankTotal;
                    suitToOrderUp = suit;
                }
            }

            return suitToOrderUp;
        }

        public virtual ICard ChooseLeadCard(IGameState gameState)
        {
            var allMyTrump = CardHelper.GetAllTrump(gameState.Trump, gameState.CurrentPlayer.Cards);

            //if I ordered up, lead a strong trump
            if (gameState.OrderingUpPlayer == gameState.CurrentPlayer && allMyTrump.Any())
            {
                if (allMyTrump.FirstOrDefault(x => x.IsTheRight(gameState.Trump)) != null)
                {
                    return allMyTrump.First(x => x.IsTheRight(gameState.Trump));
                }

                if (allMyTrump.FirstOrDefault(x => x.IsTheLeft(gameState.Trump)) != null)
                {
                    return allMyTrump.First(x => x.IsTheLeft(gameState.Trump));
                }

                return allMyTrump.OrderByDescending(x => x.Rank.Value).First();
            }

            var nonTrump = CardHelper.GetAllNonTrump(gameState.Trump, gameState.CurrentPlayer.Cards);

            if (nonTrump.Any())
            {
                return nonTrump.OrderByDescending(x => x.Rank.Value).First();
            }

            return allMyTrump.First();
        }

        public virtual ICard ChooseNonLeadCard(IGameState gameState)
        {
            //get valid indexes
            var validCards =
                CardHelper.GetValidCards(gameState.LeadSuit, gameState.Trump, gameState.CurrentPlayer.Cards).ToList();

            var nonTrumpCards = CardHelper.GetAllNonTrump(gameState.Trump, validCards).ToList();
            var trumpCards = CardHelper.GetAllTrump(gameState.Trump, validCards).ToList();

            //if partner is winning, consider throwing low
            var currentWinner = TrickHelper.PlayerWinningTrick(gameState);

            var ourTeamIsWinningTheTrick = currentWinner.TeamNumber == gameState.CurrentPlayer.TeamNumber;

            if (ourTeamIsWinningTheTrick)
            {
                return _throwJunk(nonTrumpCards, trumpCards);
            }
            else
            {
                if (trumpCards.Any())
                {
                    //do we have any trump that out ranks the highest played?
                    var highestTrumpPlayed = CardHelper.GetHighestTrump(gameState.Trump, gameState.CurrentHand.Select(x => x.Card).ToList());

                    var higherTrumpInMyHand = validCards.FirstOrDefault(x => x.IsGreaterThan(highestTrumpPlayed, gameState.Trump));

                    if (higherTrumpInMyHand != null)
                    {
                        return higherTrumpInMyHand;
                    }

                    return _throwJunk(nonTrumpCards, trumpCards);
                }
                else
                {
                    return nonTrumpCards.OrderByDescending(x => x.Rank.Value).FirstOrDefault();
                }
            }
        }

        private ICard _throwJunk(List<ICard> nonTrumpCards, List<ICard> trumpCards)
        {
            if (nonTrumpCards.Any())
            {
                return nonTrumpCards.OrderBy(x => x.Rank.Value).First();
            }

            return trumpCards.OrderBy(x => x.Rank.Value).First();
        }
    }
}
