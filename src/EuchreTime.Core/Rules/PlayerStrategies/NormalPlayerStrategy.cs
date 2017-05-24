using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Rules.PlayerStrategies
{
    public class NormalPlayerStrategy : IPlayerStrategy
    {
        public bool ShouldOrderUpDealerInFirstRound(IGameState gameState)
        {
            return false;
        }

        public ISuit SecondRoundTrumpChoice(IGameState gameState)
        {
            return null;
        }

        public ICard ChooseLeadCard(IGameState gameState)
        {
            return gameState.CurrentPlayer.Cards[0];
        }

        public ICard ChooseNonLeadCard(IGameState gameState)
        {
            return gameState.CurrentPlayer.Cards[0];
        }
    }
}
