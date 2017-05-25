using System;
using System.Linq;
using EuchreTime.Core.Game;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Bidding
{
    public class SecondRoundBidder : IHandleSecondRoundBidding
    {
        public void AskEachPlayerAboutTrump(IGameState gameState, Func<ISuit> humanChooseSuit, Action<ISuit, IPlayer> trumpSelectedCallback)
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

                    //TODO: consider an observable
                    trumpSelectedCallback(trumpSelected, gameState.CurrentPlayer);
                    break;
                }
            }
        }
    }
}
