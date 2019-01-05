using System.Collections.Generic;
using EuchreTime.Core.Players;
using MechanicGrip.Decks;
using MechanicGrip.Ranks;
using MechanicGrip.Suits;

namespace EuchreTime.Core.Rules.DealerStrategies
{
    public class FirstBlackJackDealsStrategy : IChooseDealerStrategy
    {
        public IPlayer ChooseDealer(IDeck deck, List<IPlayer> players)
        {
            //first black jack deals
            var topCard = deck.Cards.Pop();
            var playerIndex = 0;

            while (topCard.Rank.Name != StandardRank.Jack && (topCard.Suit.Equals(StandardSuit.Clubs) || topCard.Suit.Equals(StandardSuit.Spades)))
            {
                topCard = deck.Cards.Pop();

                if (playerIndex == 3)
                {
                    playerIndex = 0;
                }

                playerIndex++;
            }

            return players[playerIndex];
        }
    }
}
