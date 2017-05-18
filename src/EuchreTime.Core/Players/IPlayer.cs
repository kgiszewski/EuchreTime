﻿using System.Collections.Generic;
using EuchreTime.Core.Game;
using EuchreTime.Core.Rules.PlayerStrategies;
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
    }
}