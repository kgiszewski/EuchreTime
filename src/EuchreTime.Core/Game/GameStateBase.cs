using System.Collections.Generic;
using System.Linq;
using EuchreTime.Core.Players;
using EuchreTime.Core.Players.PlayerStrategies;
using EuchreTime.Core.Rules.DealerStrategies;
using EuchreTime.Core.Rules.WinningConditions;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Game
{
    public abstract class GameStateBase : IGameState
    {
        public IDeck Deck { get; }
        public IEnumerable<IPlayer> Players { get; }
        public int TeamOneScore { get; set; }
        public int TeamTwoScore { get; set; }
        public IPlayer Dealer { get; set; }
        public ICard TurnedUpCard { get; set; }
        public Stack<ICard> Kitty { get; set; }
        public ISuit LeadSuit { get; set; }
        public IWinningConditions WinningConditions { get; }
        public IChooseDealerStrategy ChooseDealerStrategy { get; }
        public IPlayer CurrentPlayer { get; set; }
        public ISuit Trump { get; set; }
        public IPlayer OrderingUpPlayer { get; set; }
        public List<PlayedCard> CurrentHand { get; set; }

        protected GameStateBase() : this(new EuchreDeck(), new FirstTeamToTenWins(), new FirstBlackJackDealsStrategy())
        {

        }

        protected GameStateBase(IDeck deck, IWinningConditions winningConditions, IChooseDealerStrategy dealerStrategy)
        {
            Deck = deck;
            Deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                Deck.Shuffle();
            }

            Players = new List<IPlayer> {
                new Player("Kevin", 1, new NormalPlayerStrategy(), true),
                new Player("Fred", 2, new NormalPlayerStrategy(), false),
                new Player("Sally", 1, new NormalPlayerStrategy(), false),
                new Player("Joe", 2, new NormalPlayerStrategy(), false)
            };

            WinningConditions = winningConditions;
            ChooseDealerStrategy = dealerStrategy;

            Dealer = ChooseDealerStrategy.ChooseDealer(Deck, Players.ToList());

            CurrentHand = new List<PlayedCard>();
        }

        public void AdvanceToNextPlayer()
        {
            if (CurrentPlayer == null)
            {
                //set to dealer
                CurrentPlayer = Dealer;

                return;
            }

            var playersList = Players.ToList();

            //get index of current player
            var indexOfCurrentPlayer = playersList.FindIndex(x => x.Equals(CurrentPlayer));

            var nextPlayerIndex = indexOfCurrentPlayer + 1;

            if (nextPlayerIndex == Players.Count())
            {
                nextPlayerIndex = 0;
            }

            CurrentPlayer = playersList[nextPlayerIndex];
        }

        public void SetCurrentPlayerToLeftOfDealer()
        {
            CurrentPlayer = null;
            AdvanceToNextPlayer();
            AdvanceToNextPlayer();
        }
    }
}
