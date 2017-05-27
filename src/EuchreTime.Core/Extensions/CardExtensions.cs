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
    }
}
