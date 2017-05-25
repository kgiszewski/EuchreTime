using System;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Bidding
{
    public interface IHandleFirstRoundBidding
    {
        void AskEachPlayerAboutTheTopCard(IGameState gameState, Func<bool> shouldHumanOrderUp, Action<bool> aiOrderUpCallback, Func<ICard> humanChooseDiscard);
    }
}
