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

        private static IEnumerable<Player> OrderPlayers(List<Player> players)
        {
            var orderedPlayers = players.OrderByDescending(players => players.RankScore);
            return orderedPlayers
                .Select((player, index) =>
                {
                    player.Position = index + 1;
                    return player;
                });
        }

    }
}
