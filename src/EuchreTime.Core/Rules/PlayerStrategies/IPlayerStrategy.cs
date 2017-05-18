using System.Collections.Generic;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Rules.PlayerStrategies
{
    public interface IPlayerStrategy
    {
        bool ShouldOrderUpDealerInFirstRound(IGameState gameState);
        ISuit SecondRoundTrumpChoice(IGameState gameState);
    }
}
