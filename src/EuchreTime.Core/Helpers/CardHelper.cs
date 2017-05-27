using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Extensions;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Ranks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Helpers
{
    public static class CardHelper
    {
        public static bool ContainsLeft(ISuit trumpSuit, List<ICard> cards)
        {
            var jacks = cards.Where(x => x.Rank.Symbol.ToUpper() == Rank.JackSymbol).ToList();

            if (!jacks.Any())
            {
                return false;
            }

            var oppositeSuit = GetOppositeSuit(trumpSuit);

            return jacks.Any(x => x.Suit.Equals(oppositeSuit));
        }

        public static bool ContainsRight(ISuit suit, List<ICard> cards)
        {
            var jacks = cards.Where(x => x.Rank.Symbol.ToUpper() == Rank.JackSymbol).ToList();

            return jacks.Any() && jacks.Any(x => x.Suit.Equals(suit));
        }

        public static IEnumerable<char> GetValidIndexes(ISuit leadSuit, ISuit trumpSuit, List<ICard> cards)
        {
            var validInput = new List<char>();

            //if there is a lead card AND current player has one of these cards, only offer one of these
            if (leadSuit != null &&
                (cards.Any(x => x.Suit == leadSuit) || (trumpSuit == leadSuit && ContainsLeft(trumpSuit, cards)))
            )
            {
                for (var i = 0; i < cards.Count; i++)
                {
                    var card = cards[i];

                    if (card.Suit == leadSuit || (card.IsTheLeft(trumpSuit) && trumpSuit == leadSuit))
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

        public static List<ISuit> GetSuitsToChooseFrom(ISuit turnedUpCardSuit)
        {
            var newDeck = new EuchreDeck();
            newDeck.Initialize();

            var allSuits = newDeck.Cards.Select(x => x.Suit).Distinct();

            return allSuits.Where(x => x != turnedUpCardSuit).ToList();
        }

        public static ISuit GetOppositeSuit(ISuit suit)
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

        public static List<ICard> OrderBySuitsAndRanks(this List<ICard> cards)
        {
            return cards.OrderBy(x => x.Suit.Name).ThenByDescending(x => x.Rank.Value).ToList();
        }
    }
}
