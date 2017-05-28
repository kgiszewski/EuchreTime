using System.Collections.Generic;
using EuchreTime.Core.Game;
using EuchreTime.Core.Players.PlayerStrategies;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Players
{
    public interface IPlayer
    {
        List<ICard> Cards { get; set; }
        int TeamNumber { get; }
        void Deal(IGameState gameState);
        IPlayerStrategy PlayerStrategy { get; }
        bool IsHuman { get; }
        int TricksTaken { get; set; }
        string Name { get; set; }
        void DiscardWhenOrderedUp(IGameState gameState);
        ICard ChooseCardToPlay(IGameState gameState);
        IPlayer Partner(IGameState gameState);
        bool IsDealing(IGameState gameState);
    }
}
