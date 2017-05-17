using System.Collections.Generic;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Ranks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Rules.DealerStrategies
{
    public class FirstBlackJackDealsStrategy : IChooseDealerStrategy
    {
        public IPlayer ChooseDealer(IDeck deck, List<IPlayer> players)
        {
            //first black jack deals
            var topCard = deck.Cards.Pop();
            var playerIndex = 0;

            while (topCard.Rank.Name != Rank.Jack && (topCard.Suit.Name == Suit.Clubs || topCard.Suit.Name == Suit.Spades))
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
