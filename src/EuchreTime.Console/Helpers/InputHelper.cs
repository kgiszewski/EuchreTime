using System;
using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Helpers;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Helpers
{
    public class InputHelper : IInputHelper
    {
        public char GetValidInput(string message, IEnumerable<char> validInput)
        {
            if (validInput == null)
            {
                throw new Exception("No valid input provided!");
            }

            validInput = validInput.ToList();

            var keyPressed = ' ';

            while (!validInput.Select(x => char.ToUpperInvariant(x)).Contains(keyPressed))
            {
                System.Console.WriteLine($"{message} ({string.Join(", ", validInput)})");

                keyPressed = char.ToUpperInvariant(System.Console.ReadKey(true).KeyChar);
            }
            
            return keyPressed; 
        }

        public IEnumerable<char> GetValidIndexes(ISuit leadSuit, ISuit trumpSuit, List<ICard> cards)
        {
            var validCards = CardHelper.GetValidCards(leadSuit, trumpSuit, cards);

            return validCards.Select(card => cards.IndexOf(card)).Select(index => Convert.ToChar(49 + index)).ToList();
        }
    }
}
