using System;
using EuchreTime.Core.Game;
using MechanicGrip.Core.Cards;

namespace EuchreTime.Core.Hand
{
    public interface IPlayHands
    {
        void PlayHand(IGameState gameState, Func<ICard> chooseHumanCard, Action<ICard> aiChoseCardCallback);
    }
}
