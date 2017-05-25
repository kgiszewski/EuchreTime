using System;
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

        public IEnumerable<char> GetValidIndexes(ISuit leadSuit, List<ICard> cards)
        {
            var validInput = new List<char>();

            //if there is a lead card AND current player has one of these cards, only offer one of these
            if (leadSuit != null &&
                (cards.Any(x => x.Suit == leadSuit) || ContainsLeft(leadSuit, cards))
            )
            {
                for (var i = 0; i < cards.Count; i++)
                {
                    var card = cards[i];

                    if (card.Suit == leadSuit || ContainsLeft(leadSuit, new List<ICard> { card }))
                    {
                        var charValue = Convert.ToChar(49 + i);
                        validInput.Add(charValue);
                    }
                }
            }
            else
            {
                //otherwise, any card
                for (var i = 0; i < cards.Count; i++)
                {
                    var charValue = Convert.ToChar(49 + i);
                    validInput.Add(charValue);
                }
            }

            return validInput;
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
