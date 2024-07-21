using System.Text;

using Libs.RoundRobin.Mix.Models;


namespace Libs.RoundRobin.Mix;

public partial class Planner {

    private string DTour(Tour tour, string name) {
        StringBuilder b = new($"-- Tour {name} (Rounds {tour.Rounds.Count})");
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
            return t.Players() > 0 ? $"{t.Man + 1,2}-{t.Woman + 1,-2}" : "None";
        }

    }

    private static string DPlayers(Player[] players) {
        StringBuilder b = new("Players");
        b.AppendLine();
        players.ToList().ForEach(s => {
            b.AppendLine($"-- {s.Self + 1} ({s.Played})");

            b.Append("Partners  ");
            b.Append(string.Join(", ", s.Partners.Select((v, i) => $"{i + 1}-{v}")));
            b.AppendLine($" ({s.Partners.Sum()})");

            b.Append("Opponents: Men ");
            b.Append(string.Join(",", s.OppoSame.Select((v, i) => $"{i + 1}-{v}")));
            b.Append($" ({s.OppoSame.Sum()})");

            b.Append("; Women ");
            b.Append(string.Join(",", s.OppoDiff.Select((v, i) => $"{i + 1}-{v}")));
            b.AppendLine($" ({s.OppoDiff.Sum()})");
        });
        return b.ToString();
    }

}
