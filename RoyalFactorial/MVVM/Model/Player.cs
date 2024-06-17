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
        public int GetSuitScore => CalculateSuitScore(Hand, IsTiedFirst);
        public string Name { get; } = Name;
        public int? Position { get; set; }
        public bool? IsTiedFirst { get; set; }

        public List<Card> Hand { get; } = new(Hand
            .OrderByDescending(_ => _.RankScore));

        private static int CalculateRankScore(List<Card> hand) =>
            hand.Sum(_ => _.RankScore);

        private static int CalculateSuitScore(List<Card> hand, bool? isTiedFirst)
        {
            if (!(isTiedFirst ?? false)) return -1;

            return hand.Aggregate
            (
                1,
                (product, card) => product * card.SuitScore
            );
        }
    }
}
