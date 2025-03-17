using System.Text;

using Libs.RoundRobin.Mix.Models;


namespace Libs.RoundRobin.Mix;

public partial class Planner {

    private static string DTour(Tour tour, string name) {
        StringBuilder b = new($"== Tour {name} (Rounds {tour.Rounds.Count})");
        b.AppendLine();
        b.Append(string.Join(
            Environment.NewLine,
            tour.Rounds.Select((r, i) => $"Round {i + 1, -2}: {string.Join(", ", r.Courts.Select(c => DCourt(c)))}")
        ));
        return b.ToString();

        string DCourt(Court c) {
            return $"{DTeam(c.Team1)} vs {DTeam(c.Team2)}";
        }

        string DTeam(Team t) {
            return t.Players() > 0 ? $"M{t.Man + 1,2}-{t.Woman + 1,-2}W" : "None";
        }

    }

    private static string DMixPlayers(Tour tour) {
        StringBuilder b = new();
        var valid = ValidateMix(tour);
        if (valid.Count > 0) {
            b.AppendLine(string.Join(Environment.NewLine, valid));
        } else {
            b.Append(DMixMPlayers(tour));
        }
        return b.ToString();
    }

    private static string DMixMPlayers(Tour tour) {

        var men = tour.Rounds
            .SelectMany(r => r.Courts.SelectMany(c => new int[] { c.Team1.Man, c.Team2.Man }))
            .Where( c => c >= 0)
            .GroupBy(c => c)
            .OrderBy(c => c.Key)
            .Select(c => new PlayerAdv() { Self = c.Key, Played = c.Count() })
            .ToList();

        var women = tour.Rounds
            .SelectMany(r => r.Courts.SelectMany(c => new int[] { c.Team1.Woman, c.Team2.Woman }))
            .Where(c => c >= 0)
            .GroupBy(c => c)
            .OrderBy(c => c.Key)
            .Select(c => new PlayerAdv() { Self = c.Key, Played = c.Count() })
            .ToList();

        var teams = tour.Rounds
            .SelectMany(r => r.Courts.SelectMany(c => new (int, int)[] { (c.Team1.Man, c.Team1.Woman), (c.Team2.Man, c.Team2.Woman) }))
            .ToList();

        men.ForEach(man => {
            man.Partners = women.ToDictionary(w => w.Self, w => 0);
            man.OppoSame = men.ToDictionary(w => w.Self, w => 0);
            man.OppoDiff = women.ToDictionary(w => w.Self, w => 0);

            teams
                .Where(t => t.Item1 == man.Self)
                .GroupBy(t => t.Item2)
                .ToList()
                .ForEach(t => {
                    man.Partners[t.Key] = t.Count();
                });

            //TODO Oppo Same
            //TODO Oppo Diff

        });

        women.ForEach(woman => {
            woman.Partners = men.ToDictionary(w => w.Self, w => 0);
            woman.OppoSame = women.ToDictionary(w => w.Self, w => 0);
            woman.OppoDiff = men.ToDictionary(w => w.Self, w => 0);

            teams
                .Where(t => t.Item2 == woman.Self)
                .GroupBy(t => t.Item1)
                .ToList()
                .ForEach(t => {
                    woman.Partners[t.Key] = t.Count();
                });

            //TODO Oppo Same
            //TODO Oppo Diff

        });

        StringBuilder b = new();

        b.AppendLine("Men Players");
        men.ForEach(man => {
            b.AppendLine($"-- {man.Self + 1} ({man.Played})");
            b.AppendLine($"Partners:  {string.Join(',', man.Partners.Select(s => $"{s.Key + 1}-{s.Value}"))} ({man.Partners.Sum(p => p.Value)})");
            b.AppendLine($"Opponents: Men {string.Join(',', man.OppoSame.Select(s => $"{s.Key + 1}-{s.Value}"))} ({man.OppoSame.Sum(p => p.Value)}); Women {string.Join(',', man.OppoDiff.Select(s => $"{s.Key + 1}-{s.Value}"))} ({man.OppoDiff.Sum(p => p.Value)})");
        });

        b.AppendLine("Women Players");
        women.ForEach(woman => {
            b.AppendLine($"-- {woman.Self + 1} ({woman.Played})");
            b.AppendLine($"Partners:  {string.Join(',', woman.Partners.Select(s => $"{s.Key + 1}-{s.Value}"))} ({woman.Partners.Sum(p => p.Value)})");
            b.AppendLine($"Opponents: Men {string.Join(',', woman.OppoSame.Select(s => $"{s.Key + 1}-{s.Value}"))} ({woman.OppoSame.Sum(p => p.Value)}); Women {string.Join(',', woman.OppoDiff.Select(s => $"{s.Key + 1}-{s.Value}"))} ({woman.OppoDiff.Sum(p => p.Value)})");
        });

        return b.ToString();
    }

    private static string DPlayers(Player[] players) {
        StringBuilder b = new("Players");
        b.AppendLine();
        players.ToList().ForEach(s => {
            b.AppendLine($"-- {s.Self + 1} ({s.Played})");

            b.Append("Partners:  ");
            b.Append(string.Join(",", s.Partners.Select((v, i) => $"{i + 1}-{v}")));
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
