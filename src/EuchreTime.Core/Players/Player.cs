

using EuchreTime.Core.Rules.PlayerStrategies;

namespace EuchreTime.Core.Players
{
    public class Player : PlayerBase
    {
        public Player(int teamNumber, IPlayerStrategy playerStrategy, bool isHuman) : base(teamNumber, playerStrategy, isHuman)
        {
        }
    }
}
