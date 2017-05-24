using EuchreTime.Console.Bidding;
using EuchreTime.Console.Game;
using EuchreTime.Console.Hand;
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
            var firstRoundBidder = new FirstRoundBidder(cardRenderer);
            var secondRoundBidder = new SecondRoundBidder(new SuitRenderer());
            var handPlayer = new HandPlayer(cardRenderer);

            var game = new EuchreGame(gameState, firstRoundBidder, secondRoundBidder, handPlayer);
            game.Play();
        }
    }
}
