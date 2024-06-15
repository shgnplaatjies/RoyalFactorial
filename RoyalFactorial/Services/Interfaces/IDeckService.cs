using RoyalFactorial.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.Services.Interfaces
{
    public interface IDeckService
    {
        void ShuffleDeck();
        List<List<Card>> DealCards(int numberOfPlayers, int numberOfCardsPerPlayer);
    }
}
