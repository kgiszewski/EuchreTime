using System;
using EuchreTime.Core.Game;
using MechanicGrip.Suits;

namespace EuchreTime.Core.Bidding
{
    public delegate void AiChoseTrumpHandler(object sender, AiChoseTrumpEventArgs e);

    public interface IHandleSecondRoundBidding
    {
        event AiChoseTrumpHandler OnAiChoseTrump;
        void AskEachPlayerAboutTrump(IGameState gameState, Func<ISuit> humanChooseSuit);
    }
}
