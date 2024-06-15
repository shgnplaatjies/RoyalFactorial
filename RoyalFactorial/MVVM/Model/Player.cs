using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.MVVM.Model
{
    public class Player(string Name, List<Card> Hand)
    {
        public int RankScore { get; } = CalculateRankScore(Hand);
        public int SuitScore { get; } = CalculateSuitScore(Hand);
        public string Name { get; } = Name;
        public int? Position { get; set; }

        public List<Card> Hand { get; } = new(Hand
            .OrderByDescending(_ => _.GetRankScore()));

        private static int CalculateRankScore(List<Card> hand) =>
            hand.Sum(_ => _.GetRankScore());

        private static int CalculateSuitScore(List<Card> hand) => hand
            .Aggregate
            (
                1,
                (product, card) => product * card.GetSuitScore()
            );

    }
}
