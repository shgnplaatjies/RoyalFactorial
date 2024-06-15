using RoyalFactorial.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.Services.Interfaces
{
    public interface ICardGame
    {
        List<Player> Players { get; }
        Leaderboard Leaderboard { get; }
        void InitGame(List<string> playerNames, int numberOfCardsPerPlayer, int numberOfDecks);
    }
}
