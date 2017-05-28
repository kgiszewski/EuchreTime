using System.Linq;
using EuchreTime.Core.Extensions;
using EuchreTime.Core.Game;
using EuchreTime.Core.Players;

namespace EuchreTime.Core.Helpers
{
    public static class TrickHelper
    {
        public static IPlayer PlayerWinningTrick(IGameState gameState)
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

            return winner;
        }
    }
}
