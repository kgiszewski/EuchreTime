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

        public static bool ContainsRight(ISuit trumpSuit, List<ICard> cards)
        {
            var jacks = cards.Where(x => x.Rank.Symbol.ToUpper() == Rank.JackSymbol).ToList();

            return jacks.Any() && jacks.Any(x => x.Suit.Equals(trumpSuit));
        }
        
        public static IEnumerable<ICard> GetValidCards(ISuit leadSuit, ISuit trumpSuit, List<ICard> cards)
        {
            var validCards = new List<ICard>();

            //if there is a lead card AND current player has one of these cards, only offer one of these
            if (leadSuit != null && leadSuit != trumpSuit && cards.Any(x => x.Suit == leadSuit)
            )
            {
                validCards.AddRange(GetAllNonTrump(trumpSuit, cards).Where(x =>x.Suit == leadSuit));
            }
            else if (trumpSuit == leadSuit)
            {
                validCards.AddRange(GetAllTrump(trumpSuit, cards));
            }
            
            if(!validCards.Any())
            {
                //otherwise, any card
                validCards.AddRange(cards);
            }

            return validCards;
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

        public static List<ICard> GetAllTrump(ISuit trumpSuit, List<ICard> cards)
        {
            return cards.Where(x => x.Suit == trumpSuit || x.IsTheLeft(trumpSuit)).ToList();
        }

        public static List<ICard> GetAllNonTrump(ISuit trumpSuit, List<ICard> cards)
        {
            return cards.Where(x => x.Suit != trumpSuit && !x.IsTheLeft(trumpSuit)).ToList();
        }

        public static ICard GetHighestTrump(ISuit trumpSuit, List<ICard> cards)
        {
            if(ContainsRight(trumpSuit, cards))
            {
                return cards.First(x => x.IsTheRight(trumpSuit));
            }

            if (ContainsLeft(trumpSuit, cards))
            {
                return cards.First(x => x.IsTheLeft(trumpSuit));
            }

            return cards.OrderByDescending(x => x.Rank.Value).FirstOrDefault(x => x.Suit == trumpSuit);
        }
    }
}
