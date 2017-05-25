using EuchreTime.Console.Bidding;
using EuchreTime.Console.Game;
using EuchreTime.Console.Hand;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;

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
            var firstRoundBidder = new FirstRoundBidder(cardRenderer, inputHelper);
            var secondRoundBidder = new SecondRoundBidder(new SuitRenderer(), inputHelper);
            var handPlayer = new HandPlayer(cardRenderer, inputHelper);

            var game = new EuchreGame(gameState, firstRoundBidder, secondRoundBidder, handPlayer);
            game.Play();
        }
    }
}
