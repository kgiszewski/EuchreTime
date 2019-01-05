using System.Collections.Generic;
using System.ComponentModel;
using EuchreTime.Core.Players;
using EuchreTime.Core.Rules.DealerStrategies;
using EuchreTime.Core.Rules.WinningConditions;
using MechanicGrip.Cards;
using MechanicGrip.Decks;
using MechanicGrip.Suits;

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
        ISuit LeadSuit { get; set; }
        IWinningConditions WinningConditions { get; }
        IChooseDealerStrategy ChooseDealerStrategy { get; }
        IPlayer CurrentPlayer { get; set; }
        void AdvanceToNextPlayer();
        void SetCurrentPlayerToLeftOfDealer();
        ISuit Trump { get; set; }
        IPlayer OrderingUpPlayer { get; set; }
        List<PlayedCard> CurrentHand { get; set; }
    }
}
