using EuchreTime.Core.Game;

namespace EuchreTime.Console.Bidding
{
    public interface IHandleSecondRoundBidding
    {
        void AskEachPlayerAboutTrump(IGameState gameState);
    }
}
