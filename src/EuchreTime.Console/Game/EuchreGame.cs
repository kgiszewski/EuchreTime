using EuchreTime.Console.Helpers;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Bidding;
using EuchreTime.Core.Game;
using EuchreTime.Core.Hand;
using EuchreTime.Core.Helpers;

namespace EuchreTime.Console.Game
{
    public class EuchreGame : EuchreGameBase
    {
        public EuchreGame(
            IGameState gameState, 
            IHandleFirstRoundBidding firstRoundBidder, 
            IHandleSecondRoundBidding secondRoundBidder,
            IPlayHands handPlayer,
            IRenderCards cardRenderer,
            IInputHelper inputHelper,
            IRenderSuits suitRenderer
        ) 
            : base(gameState, firstRoundBidder, secondRoundBidder, handPlayer, cardRenderer, inputHelper, suitRenderer)
        {
        }
    }
}
