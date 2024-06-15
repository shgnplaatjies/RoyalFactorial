using RoyalFactorial.MVVM.Model;
using RoyalFactorial.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.Services
{
    public class CardGame : ICardGame
    {
        public List<Player> Players { get; private set; } = null!;

        public Leaderboard Leaderboard { get; private set; } = null!;

        public void InitGame(List<string> playerNames, int numberOfCardsPerPlayer, int numberOfDecks)
        {
            var deckService = new DeckService(numberOfDecks);

            deckService.ShuffleDeck();

            var hands = deckService.DealCards(playerNames.Count, numberOfCardsPerPlayer);

            Players = new(
                playerNames
                .Select(
                    (name, index) => new Player(name, hands[index])
                    )
                );

            Leaderboard = new(Players);
        }
    }
}
