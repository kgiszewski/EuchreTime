using EuchreTime.Console.Bidding;
using EuchreTime.Console.Hand;
using EuchreTime.Core.Game;

namespace EuchreTime.Console.Game
{
    public class EuchreGame : EuchreGameBase
    {
        public EuchreGame(
            IGameState gameState, 
            IHandleFirstRoundBidding firstRoundBidder, 
            IHandleSecondRoundBidding secondRoundBidder,
            IPlayHands handPlayer
        ) 
            : base(gameState, firstRoundBidder, secondRoundBidder, handPlayer)
        {
        }
    }
}
