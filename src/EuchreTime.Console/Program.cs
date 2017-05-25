using EuchreTime.Console.Game;
using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Bidding;
using EuchreTime.Core.Game;
using EuchreTime.Core.Hand;
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
            var firstRoundBidder = new FirstRoundBidder();
            var secondRoundBidder = new SecondRoundBidder();
            var handPlayer = new HandPlayer();

            var game = new EuchreGame(
                gameState, 
                firstRoundBidder, 
                secondRoundBidder, 
                handPlayer, 
                cardRenderer, 
                inputHelper, 
                suitRenderer
            );

            game.Play();
        }
    }
}
