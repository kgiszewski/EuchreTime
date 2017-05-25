using System;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Bidding
{
    public delegate void AiOrderedUpDealerHandler(object sender, AiOrderedUpDealerEventArgs e);

    public interface IHandleFirstRoundBidding
    {
        event AiOrderedUpDealerHandler OnAiOrderedUpDealer;

        void AskEachPlayerAboutTheTopCard(IGameState gameState, Func<bool> shouldHumanOrderUp, Func<ICard> humanChooseDiscard);
    }
}
