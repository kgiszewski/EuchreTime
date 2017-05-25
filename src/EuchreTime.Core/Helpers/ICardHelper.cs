using System.Collections.Generic;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Helpers
{
    public interface ICardHelper
    {
        bool ContainsLeft(ISuit suit, List<ICard> cards);
        bool ContainsRight(ISuit suit, List<ICard> cards);
        IEnumerable<char> GetValidIndexes(ISuit leadSuit, List<ICard> cards);
    }
}