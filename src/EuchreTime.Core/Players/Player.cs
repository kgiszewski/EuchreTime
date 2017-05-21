

using EuchreTime.Core.Rules.PlayerStrategies;

namespace EuchreTime.Core.Players
{
    public class Player : PlayerBase
    {
        public Player(string name, int teamNumber, IPlayerStrategy playerStrategy, bool isHuman) 
            : base(name, teamNumber, playerStrategy, isHuman)
        {
        }
    }
}
