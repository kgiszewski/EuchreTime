using EuchreTime.Console.Bidding;
using EuchreTime.Console.Game;
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
            var firstRoundBidder = new FirstRoundBidder(new CardRenderer());
            var secondRoundBidder = new SecondRoundBidder(new SuitRenderer());

            var game = new EuchreGame(gameState, firstRoundBidder, secondRoundBidder);
            game.Play();
        }
    }
}
