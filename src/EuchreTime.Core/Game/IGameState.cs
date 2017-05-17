using System.Collections.Generic;
using EuchreTime.Core.Players;
using EuchreTime.Core.Rules.DealerStrategies;
using EuchreTime.Core.Rules.WinningConditions;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Game
{
    public interface IGameState
    {
        IDeck Deck { get; }
        IEnumerable<IPlayer> Players { get; }
        int TeamOneScore { get; set; }
        int TeamTwoScore { get; set; }
        IPlayer Dealer { get; set; }
        ICard TurnedUpCard { get; set; }
        Stack<ICard> Kitty { get; set; }
        Suit LeadSuit { get; set; }
        IWinningConditions WinningConditions { get; }
        IChooseDealerStrategy ChooseDealerStrategy { get; }
    }
}
