﻿using System.Collections.Generic;
using System.Linq;
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
            //return gameState.CurrentPlayer.Cards.First().Suit;
        }
    }
}
