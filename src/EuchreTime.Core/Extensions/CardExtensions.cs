using EuchreTime.Core.Helpers;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Ranks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Extensions
{
    public static class CardExtensions
    {
        public static bool IsTheRight(this ICard card, ISuit trumpSuit)
        {
            return card.Rank.Symbol == Rank.JackSymbol && card.Suit == trumpSuit;
        }

        public static bool IsTheLeft(this ICard card, ISuit trumpSuit)
        {
            return card.Rank.Symbol == Rank.JackSymbol && card.Suit == CardHelper.GetOppositeSuit(trumpSuit);
        }

        public static bool IsTrump(this ICard card, ISuit trumpSuit)
        {
            return card.Suit == trumpSuit;
        }

        public static bool IsGreaterThan(this ICard card, ICard otherCard, ISuit trumpSuit)
        {
            if (otherCard == null)
            {
                return true;
            }

            if (card.IsTrump(trumpSuit) && !otherCard.IsTrump(trumpSuit))
            {
                return true;
            }

            if (!card.IsTrump(trumpSuit) && otherCard.IsTrump(trumpSuit))
            {
                return false;
            }

            //handle bowers
            if (card.IsTrump(trumpSuit) && otherCard.IsTrump(trumpSuit))
            {
                if (card.IsTheRight(trumpSuit))
                {
                    return true;
                }

                if (otherCard.IsTheRight(trumpSuit))
                {
                    return false;
                }

                if (card.IsTheLeft(trumpSuit) && !otherCard.IsTheRight(trumpSuit))
                {
                    return true;
                }

                if (!card.IsTheRight(trumpSuit) && otherCard.IsTheLeft(trumpSuit))
                {
                    return false;
                }
            }

            //we need to compare ranks
            if (card.Suit == otherCard.Suit)
            {
                return card.Rank.Value > otherCard.Rank.Value;
            }

            //we should get here if we compare two non-trump suits
            return false;
        }
    }
}
