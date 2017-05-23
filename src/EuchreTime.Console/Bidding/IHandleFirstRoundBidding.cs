using EuchreTime.Core.Game;

namespace EuchreTime.Console.Bidding
{
    public interface IHandleFirstRoundBidding
    {
        void AskEachPlayerAboutTheTopCard(IGameState gameState);
    }
}
