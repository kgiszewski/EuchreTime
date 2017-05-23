using EuchreTime.Console.Bidding;
using EuchreTime.Core.Game;

namespace EuchreTime.Console.Game
{
    public class EuchreGame : EuchreGameBase
    {
        public EuchreGame(IGameState gameState, IHandleFirstRoundBidding firstRoundBidder, IHandleSecondRoundBidding secondRoundBidder) : base(gameState, firstRoundBidder, secondRoundBidder)
        {
        }
    }
}
