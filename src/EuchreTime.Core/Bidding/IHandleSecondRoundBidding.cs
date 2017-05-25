using System;
using EuchreTime.Core.Game;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Bidding
{
    public interface IHandleSecondRoundBidding
    {
        void AskEachPlayerAboutTrump(IGameState gameState, Func<ISuit> humanChooseSuit, Action<ISuit, IPlayer> trumpSelectedCallback);
    }
}
