using System.Collections.Generic;
using EuchreTime.Core.Players;
using EuchreTime.Core.Rules.DealerStrategies;
using EuchreTime.Core.Rules.WinningConditions;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Game
{
    public class GameState
    {
        private readonly List<Player> _players;
        private readonly Player _dealer;
        private readonly IDeck _deck;
        private readonly IWinningConditions _winningConditions;
        private readonly IChooseDealerStrategy _dealerStrategy;

        public int TeamOneScore = 0;
        public int TeamTwoScore = 0;

        public Card TurnedUpCard;
        public Stack<Card> Kitty;
       
        public Suit LeadSuit;

        public GameState() : this(new EuchreDeck(), new NormalWinningConditions(), new DealerChooser())
        {
            
        }

        public GameState(IDeck deck, IWinningConditions winningConditions, IChooseDealerStrategy dealerStrategy)
        {
            _deck = deck;
            _deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                _deck.Shuffle();
            }

            _players = new List<Player> { new Player(1), new Player(2), new Player(1), new Player(2) };

            _winningConditions = winningConditions;
            _dealerStrategy = dealerStrategy;

            _dealer = _dealerStrategy.ChooseDealer(_deck, _players);
        }

        public Player GetDealer()
        {
            return _dealer;
        }

        public IDeck GetDeck()
        {
            return _deck;
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _players;
        }

        public IWinningConditions GetWinningConditions()
        {
            return _winningConditions;
        }
    }
}
