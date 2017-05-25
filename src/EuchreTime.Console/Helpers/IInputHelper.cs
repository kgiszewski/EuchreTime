using System.Collections.Generic;

namespace EuchreTime.Console.Helpers
{
    public interface IInputHelper
    {
        char GetValidInput(string message, IEnumerable<char> validInput);
    }
}
