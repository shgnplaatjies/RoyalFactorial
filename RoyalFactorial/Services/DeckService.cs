using RoyalFactorial.MVVM.Model;
using RoyalFactorial.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.Services
{
    public class DeckService : IDeckService
    {
        private List<Card> _deck;
        public int NumberOfDecks { get; }

        public DeckService(int numberOfDecks = 1)
        {
            List<Rank> availableRanks = Card.GetRanks();
            List<Suit> availableSuits = Card.GetSuits();
            this.NumberOfDecks = numberOfDecks;

            _deck = (from rank in availableRanks
                     from suit in availableSuits
                     from deck in Enumerable.Range(0, numberOfDecks)
                     select new Card(suit, rank)).ToList();
        }

        public void ShuffleDeck() =>
            _deck = new List<Card>(_deck.OrderBy(_ => Guid.NewGuid()));

        public List<List<Card>> DealCards(int numberOfPlayers, int numberOfCardsPerPlayer)
        {
            if (numberOfPlayers < 1)
                return new(([]));

            var numberOfCardsRequired = numberOfPlayers * numberOfCardsPerPlayer;

            if (numberOfCardsRequired > _deck.Count)
                return new(Enumerable.Range(0, numberOfPlayers).Select(_ => new List<Card>()));

            var undealtCards = _deck.Skip(numberOfCardsRequired).ToList();

            _deck = undealtCards;

            var players = Enumerable.Range(0, numberOfPlayers);

            return players
                .Select(
                    i => _deck.Skip(i * numberOfCardsPerPlayer)
                        .Take(numberOfCardsPerPlayer)
                        .ToList()
                ).ToList();
        }
    }
}
