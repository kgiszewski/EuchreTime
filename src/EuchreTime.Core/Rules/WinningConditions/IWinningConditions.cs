using EuchreTime.Core.Game;

namespace EuchreTime.Core.Rules.WinningConditions
{
    public interface IWinningConditions
    {
        bool HasAnyTeamWon(IGameState gameState);
    }
}
