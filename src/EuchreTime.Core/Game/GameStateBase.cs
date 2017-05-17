using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Players;
using EuchreTime.Core.Rules.DealerStrategies;
using EuchreTime.Core.Rules.WinningConditions;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Game
{
    public class GameStateBase : IGameState
    {
        public IDeck Deck { get; }
        public IEnumerable<IPlayer> Players { get; }
        public int TeamOneScore { get; set; }
        public int TeamTwoScore { get; set; }
        public IPlayer Dealer { get; set; }
        public ICard TurnedUpCard { get; set; }
        public Stack<ICard> Kitty { get; set; }
        public Suit LeadSuit { get; set; }
        public IWinningConditions WinningConditions { get; }
        public IChooseDealerStrategy ChooseDealerStrategy { get; }

        public GameStateBase() : this(new EuchreDeck(), new FirstTeamToTenWins(), new FirstBlackJackDealsStrategy())
        {

        }

        public GameStateBase(IDeck deck, IWinningConditions winningConditions, IChooseDealerStrategy dealerStrategy)
        {
            Deck = deck;
            Deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                Deck.Shuffle();
            }

            Players = new List<IPlayer> { new Player(1), new Player(2), new Player(1), new Player(2) };

            WinningConditions = winningConditions;
            ChooseDealerStrategy = dealerStrategy;

            Dealer = ChooseDealerStrategy.ChooseDealer(Deck, Players.ToList());
        }
    }
}
