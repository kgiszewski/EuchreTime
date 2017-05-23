using System.Collections.Generic;
using System.Linq;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Helpers
{
    public class CardHelper : ICardHelper
    {
        public bool ContainsLeft(ISuit suit, List<ICard> cards)
        {
            var jacks = cards.Where(x => x.Rank.Symbol.ToUpper() == "J").ToList();

            if (!jacks.Any())
            {
                return false;
            }

            var oppositeSuit = _getOppositeSuit(suit);

            return jacks.Any(x => x.Suit.Equals(oppositeSuit));
        }

        public bool ContainsRight(ISuit suit, List<ICard> cards)
        {
            var jacks = cards.Where(x => x.Rank.Symbol.ToUpper() == "J").ToList();

            return jacks.Any() && jacks.Any(x => x.Suit.Equals(suit));
        }

        private ISuit _getOppositeSuit(ISuit suit)
        {
            if (suit.Equals(Suit.Clubs))
            {
                return Suit.Spades;
            }

            if (suit.Equals(Suit.Spades))
            {
                return Suit.Clubs;
            }

            if (suit.Equals(Suit.Hearts))
            {
                return Suit.Diamonds;
            }

            return Suit.Hearts;
        }
    }
}
