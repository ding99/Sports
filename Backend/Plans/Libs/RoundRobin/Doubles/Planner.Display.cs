using System.Text;

using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    private static string DTour(Tour tour, string name) {
        StringBuilder b = new($"-- Tour {name} (Rounds {tour.Rounds.Count})");
        int r = 0;
        b.AppendLine();
        b.AppendLine(string.Join(
            Environment.NewLine,
            tour.Rounds.Select((r, i) => $"Round{i + 1, -2}: {string.Join(", ", r.Courts.Select(c => DCourt(c)))}")
        ));
        return b.ToString();

        string DCourt(Court c) {
            return $"{DTeam(c.Team1)} vs {DTeam(c.Team2)}";
        }

        string DTeam(Team t) {
            return t.Players.Count > 1 ? $"{t.Players[0] + 1,2}-{t.Players[1] + 1,-2}" : t.Players[0].ToString();
        }

    }

    private static string DPlayers(Player[] players) {
        StringBuilder b = new();
        players.ToList().ForEach(s => {
            b.AppendLine($"-- {s.Self + 1} ({s.Played})");

            b.Append("Partners  ");
            b.Append(string.Join(", ", s.Partners.Select((v, i) => $"{i + 1}-{v}")));
            b.AppendLine($" ({s.Partners.Sum()})");

            b.Append("Opponents ");
            b.Append(string.Join(", ", s.Opponents.Select((v, i) => $"{i + 1}-{v}")));
            b.AppendLine($" ({s.Opponents.Sum()})");
        });
        return b.ToString();
    }

}
