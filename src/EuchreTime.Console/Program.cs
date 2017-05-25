using EuchreTime.Console.Bidding;
using EuchreTime.Console.Game;
using EuchreTime.Console.Hand;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using EuchreTime.Core.Helpers;

namespace EuchreTime.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //init
            var gameState = new GameState();
            var cardRenderer = new CardRenderer();
            var inputHelper = new InputHelper();
            var suitRenderer = new SuitRenderer();
            var cardHelper = new CardHelper();
            var firstRoundBidder = new FirstRoundBidder(cardRenderer, inputHelper);
            var secondRoundBidder = new SecondRoundBidder(suitRenderer, inputHelper);
            var handPlayer = new HandPlayer(cardRenderer, inputHelper, cardHelper);

            var game = new EuchreGame(gameState, firstRoundBidder, secondRoundBidder, handPlayer);

            game.Play();
        }
    }
}
