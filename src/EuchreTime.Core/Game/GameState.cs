using System.Collections.Generic;
using EuchreTime.Core.Players;
using MechanicGrip.Core.Cards;
using MechanicGrip.Core.Decks;
using MechanicGrip.Core.Ranks;
using MechanicGrip.Core.Suits;

namespace EuchreTime.Core.Game
{
    public class GameState
    {
        private readonly List<Player> _players;
        private Player _dealer;
        private EuchreDeck _deck;

        public int TeamOneScore = 0;
        public int TeamTwoScore = 0;

        public Stack<Card> Kitty;

        public Suit Trump
        {
            get;
            set;
        }

        public GameState()
        {
            _deck.Initialize();

            for (var i = 0; i < 100; i++)
            {
                _deck.Shuffle();
            }

            _players = new List<Player> { new Player(1), new Player(2), new Player(1), new Player(2)};

            var cards = _deck.GetDeck();

            //first black jack deals
            var topCard = cards.Pop();
            var playerIndex = 0;

            while (topCard.Rank.Name != Rank.Jack && (topCard.Suit.Name == Suit.Clubs || topCard.Suit.Name == Suit.Spades))
            {
                topCard = cards.Pop();

                if (playerIndex == 3)
                {
                    playerIndex = 0;
                }

                playerIndex++;
            }

            _dealer = _players[playerIndex];
        }

        public bool HasAnyTeamWon()
        {
            return TeamOneScore >= 10 || TeamTwoScore >= 10;
        }

        public Player GetDealer()
        {
            return _dealer;
        }

        public EuchreDeck GetDeck()
        {
            return _deck;
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _players;
        }
    }
}
