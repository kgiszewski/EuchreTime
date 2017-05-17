using System.Collections.Generic;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Ranks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Rules.DealerStrategies
{
    public class DealerChooser : IChooseDealerStrategy
    {
        public Player ChooseDealer(IDeck deck, List<Player> players)
        {
            var cards = deck.GetCards();

            //first black jack deals
            var topCard = cards.Pop();
            var playerIndex = 0;

            while (topCard.Rank.Name != Rank.Jack && (topCard.Suit.Name == Suit.Clubs || topCard.Suit.Name == Suit.Spades))
            {
                topCard = cards.Pop();

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
