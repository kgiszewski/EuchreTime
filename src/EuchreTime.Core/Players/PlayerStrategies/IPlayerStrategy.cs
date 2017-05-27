using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Players.PlayerStrategies
{
    public interface IPlayerStrategy
    {
        bool ShouldOrderUpDealerInFirstRound(IGameState gameState);
        ISuit SecondRoundTrumpChoice(IGameState gameState);
        ICard ChooseLeadCard(IGameState gameState);
        ICard ChooseNonLeadCard(IGameState gameState);
    }
}
