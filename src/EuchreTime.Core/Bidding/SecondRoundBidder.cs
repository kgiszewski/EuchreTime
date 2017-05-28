using System;
using System.Linq;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Bidding
{
    public class SecondRoundBidder : IHandleSecondRoundBidding
    {
        public event AiChoseTrumpHandler OnAiChoseTrump;

        public void AskEachPlayerAboutTrump(IGameState gameState, Func<ISuit> humanChooseSuit)
        {
            for (var i = 0; i < gameState.Players.Count(); i++)
            {
                gameState.AdvanceToNextPlayer();

                ISuit trumpSelected = null;

                if (gameState.CurrentPlayer.IsHuman)
                {
                    trumpSelected = humanChooseSuit();
                }
                else
                {
                    trumpSelected = gameState.CurrentPlayer.PlayerStrategy.SecondRoundTrumpChoice(gameState);
                }

                if (trumpSelected != null)
                {
                    gameState.Trump = trumpSelected;
                    gameState.OrderingUpPlayer = gameState.CurrentPlayer;

                    OnAiChoseTrump?.Invoke(this, new AiChoseTrumpEventArgs(gameState.CurrentPlayer, trumpSelected));
                    break;
                }
            }
        }
    }
}
