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
        public int NumDecks { get; }

        public DeckService(int numDecks)
        {
            List<Rank> availableRanks = Card.GetRanks();
            List<Suit> availableSuits = Card.GetSuits();
            this.NumDecks = numDecks;

            _deck = (from rank in availableRanks
                     from suit in availableSuits
                     from deck in Enumerable.Range(0, numDecks)
                     select new Card(suit, rank)).ToList();
        }

        public void ShuffleDeck() =>
            _deck = new List<Card>(_deck.OrderBy(_ => Guid.NewGuid()));

        public List<List<Card>> DealCards(int numPlayers, int numCardsPerPlayer)
        {
            var numCardsRequired = numPlayers * numCardsPerPlayer;

            if (numCardsRequired > _deck.Count)
                throw new InvalidOperationException(
                    $"Not enough cards available for all players." +
                    $"Required: {numCardsRequired}," +
                    $"Available: {_deck.Count}");

            var undealtCards = _deck.Skip(numCardsRequired).ToList();

            _deck = undealtCards;

            var players = Enumerable.Range(0, numPlayers);

            return players
                .Select(
                    i => _deck.Skip(i * numCardsPerPlayer)
                        .Take(numCardsPerPlayer)
                        .ToList()
                ).ToList();
        }
    }
}
