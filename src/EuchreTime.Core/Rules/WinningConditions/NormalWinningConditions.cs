using EuchreTime.Core.Game;

namespace EuchreTime.Core.Rules.WinningConditions
{
    public class NormalWinningConditions : IWinningConditions
    {
        public bool HasAnyTeamWon(IGameState gameState)
        {
            return gameState.TeamOneScore >= 10 || gameState.TeamTwoScore >= 10;
        }
    }
}
