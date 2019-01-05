using System.Collections.Generic;
using MechanicGrip.Cards;
using MechanicGrip.Suits;

namespace EuchreTime.Console.Helpers
{
    public interface IInputHelper
    {
        char GetValidInput(string message, IEnumerable<char> validInput);

        IEnumerable<char> GetValidIndexes(ISuit leadSuit, ISuit trumpSuit, List<ICard> cards);
    }
}
