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
            var numberOfCardsRequired = numberOfPlayers * numberOfCardsPerPlayer;

            var players = Enumerable.Range(0, numberOfPlayers);

            if (numberOfCardsRequired > _deck.Count)
                return new(players.Select(_ => new List<Card>()));

            return new(players
                .Select(
                    (_, i) => _deck.Skip(i * numberOfCardsPerPlayer)
                        .Take(numberOfCardsPerPlayer)
                        .ToList()
                ));
        }
    }
}
