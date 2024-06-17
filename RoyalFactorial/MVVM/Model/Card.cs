using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.MVVM.Model
{
    public record Card(Suit Suit, Rank Rank)
    {
        public int RankScore { get; } = Rank.Score;

        public int SuitScore { get; } = Suit.Score;
        public override string ToString() => $"Card<{Rank},{Suit}>";

        public static List<Rank> GetRanks() =>
        [
            new ("Two", "2", 2),
            new ("Three", "3", 3),
            new ("Four", "4", 4),
            new ("Five", "5", 5),
            new ("Six", "6", 6),
            new ("Seven", "7", 7),
            new ("Eight", "8", 8),
            new ("Nine", "9", 9),
            new ("Ten", "10", 10),
            new ("Ace", "A", 11),
            new ("Jack", "J", 11),
            new ("Queen", "Q", 12),
            new ("King", "K", 13),
        ];

        public static List<Suit> GetSuits() =>
        [
            new ("Diamonds", "♦", 1),
            new ("Hearts", "♥", 2),
            new ("Spades", "♠", 3),
            new ("Clubs", "♣", 4),
        ];
    }
}
