using System.Linq;
using EuchreTime.Console.Rendering;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Console.Bidding
{
    public class SecondRoundBidder : IHandleSecondRoundBidding
    {
        private readonly IRenderSuits _suitRenderer;

        public SecondRoundBidder(IRenderSuits suitRenderer)
        {
            _suitRenderer = suitRenderer;
        }

        public void AskEachPlayerAboutTrump(IGameState gameState)
        {
            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                ISuit trumpSelected = null;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    var renderedSuits = _suitRenderer.RenderSuits(gameState);

                    System.Console.WriteLine(renderedSuits);

                    //prompt for a suit to choose or pass
                    trumpSelected = null;
                }
                else
                {
                    trumpSelected = gameState.CurrentPlayer.PlayerStrategy.SecondRoundTrumpChoice(gameState);
                }

                if (trumpSelected != null)
                {
                    //set trump
                    gameState.Trump = trumpSelected;
                    break;
                }
            }
        }
    }
}
