using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyalFactorial.MVVM.Model
{
    public record Leaderboard(List<Player> Players)
    {
        public List<Player> Players { get; } = OrderPlayers(Players).ToList();

        private static IEnumerable<Player> SetTiedFirst(List<Player> players)
        {
            if (players.Count == 0) return players;


            var topScore = players[0].RankScore;
            bool isTied = players.Count(player => player.RankScore == topScore) > 1;

            if (!isTied)
                return players
                    .Select(
                    player =>
                    {
                        player.IsTiedFirst = false;
                        return player;
                    });

            return players
                .Select(
                player =>
                {
                    player.IsTiedFirst = player.RankScore == topScore;
                    return player;
                });
        }

        private static IEnumerable<Player> OrderPlayers(List<Player> players)
        {
            var orderedPlayers = SetTiedFirst(new(players.OrderByDescending(_ => _.RankScore)));

            return orderedPlayers
                .Select((player, index) =>
                {
                    if (player.IsTiedFirst == true || index == 0)
                        player.Position = 1;
                    else
                        player.Position = orderedPlayers.ElementAt(index - 1).Position + 1;

                    return player;
                });
        }

    }
}
