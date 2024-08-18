using CSharpFunctionalExtensions;

using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    /// <summary>
    /// Create a double tournament
    /// </summary>
    /// <param name="persons">Number of players</param>
    /// <param name="games">Number of person-time</param>
    /// <param name="detail">Show details in log</param>
    /// <param name="sample">Sample tournament</param>
    /// <returns></returns>
    public Result<(Tour t, Player[] p, string mst)> CreateDbl(int persons, int games, bool detail, bool sample) {
        var result = Pair(persons, games, detail, sample);

        if (result.IsSuccess) {
            if (detail) {
                log.Information("{d}", DTour(result.Value.oall.Tour, $"{persons}/{games}Games"));
                log.Information("{d}", DPlayers(result.Value.oall.Players));
            }
            return (result.Value.oall.Tour, result.Value.oall.Players, result.Value.mst);
        } else {
            log.Error("Failed to create double: {err}", result.Error);
            return Result.Failure<(Tour, Player[], string)>(result.Error);
        }
    }

    public Result<(Overall oall, string mst)> Pair(int persons, int games, bool detail, bool sample) {
        if (games % persons > 0) {
            return Result.Failure<(Overall, string)>($"games {games} should be a multiple of persons {persons}!");
        }
        if (games % 4 > 0) {
            return Result.Failure<(Overall, string)>($"games {games} should be a multiple of 4!");
        }

        var master = CreateMaster(persons, games, sample);

        var oa = new Overall(persons, games);
        int count;

        string mst = $"({master.Count}) {string.Join(',', master)}";

        try {
            while ((count = master.Count) > 0) {

                var list = master
                    .Select((d, i) => new Order(i, d))
                    .Where(x => !oa.Round.Contain(x.Person) && !oa.Court.Contain(x.Person));

                list = GetMinPlay(list, oa.Players);
                list = GetMinOppo(list, oa);
                list = GetMinPart(list, oa);

                UpdateList(oa, master, list);
            }

            if (oa.Court.Players() > 0) {
                oa.Round.Courts.Add(oa.Court);
            }

            if (oa.Round.Courts.Count > 0) {
                oa.Tour.Rounds.Add(oa.Round);
            }

        } catch (Exception e) {
            return Result.Failure<(Overall, string)>(e.Message + e.StackTrace);
        }

        if (detail) {
            log.Information("List ({ct})  {list}", master.Count, string.Join(", ", master));
        }

        return (oa, mst);
    }

    #region chose

    public static IEnumerable<Order> GetMinPlay(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    public static IEnumerable<Order> GetMinOppo(IEnumerable<Order> list, Overall oa) {
        if (oa.Court.Players() < 2) {
            return list;
        }

        Player[] players = oa.Players;
        var opp1 = oa.Court.Team1.Members[0];
        var opp2 = oa.Court.Team1.Members[1];

        List<Order> midd = [];
        int? ops = null;
        foreach (var o in list) {
            if (!ops.HasValue || players[opp1].Opponents[o.Person] + players[opp2].Opponents[o.Person] < ops) {
                ops = players[opp1].Opponents[o.Person] + players[opp2].Opponents[o.Person];
                midd = [new Order(o.Index, o.Person)];
            } else if (players[opp1].Opponents[o.Person] + players[opp2].Opponents[o.Person] == ops) {
                midd.Add(new Order(o.Index, o.Person));
            }
        }

        List<Order> mins = [];
        ops = null;
        foreach (var o in midd) {
            if (!ops.HasValue || Math.Abs(players[opp1].Opponents[o.Person] - players[opp2].Opponents[o.Person]) < ops) {
                ops = Math.Abs(players[opp1].Opponents[o.Person] - players[opp2].Opponents[o.Person]);
                mins = [new Order(o.Index, o.Person)];
            } else if (Math.Abs(players[opp1].Opponents[o.Person] - players[opp2].Opponents[o.Person]) == ops) {
                mins.Add(new Order(o.Index, o.Person));
            }
        }

        return mins;
    }

    public static IEnumerable<Order> GetMinPart(IEnumerable<Order> list, Overall oa) {
        if ((oa.Court.Players() & 1) < 1) {
            return list;
        }

        List<Order> mins = [];
        int? parts = null;
        var psn = oa.Court.Players() == 1 ? oa.Court.Team1.Members[0] : oa.Court.Team2.Members[0];

        foreach (var m in list) {
            if (!parts.HasValue || oa.Players[m.Person].Partners[psn] < parts) {
                parts = oa.Players[m.Person].Partners[psn];
                mins = [new Order(m.Index, m.Person)];
            } else if (oa.Players[m.Person].Partners[psn] == parts) {
                mins.Add(new Order(m.Index, m.Person));
            }
        }

        return mins.GroupBy(o => o.Index, (o, g) => g.First()).ToList();
    }

    #endregion

    #region util

    public static void UpdateList(Overall oa, List<int> master, IEnumerable<Order> list) {
        var min = list.First();
        if (min != null) {
            AddPlayer(oa, min.Person);
            master.RemoveAt(min.Index);
        }
    }

    private static void AddPlayer(Overall oa, int p) {
        var self = oa.Players[p];
        self.Played++;
        switch (oa.Court.Players()) {
        case 0:
            oa.Court.Team1.Members.Add(p);
            break;
        case 1:
            var part1 = oa.Players[oa.Court.Team1.Members[0]];
            self.Partners[oa.Court.Team1.Members[0]]++;
            part1.Partners[p]++;
            oa.Court.Team1.Members.Add(p);
            break;
        case 2:
            oa.Players[oa.Court.Team1.Members[0]].Opponents[p]++;
            oa.Players[oa.Court.Team1.Members[1]].Opponents[p]++;
            self.Opponents[oa.Court.Team1.Members[0]]++;
            self.Opponents[oa.Court.Team1.Members[1]]++;
            oa.Court.Team2.Members.Add(p);
            break;
        case 3:
            oa.Players[oa.Court.Team1.Members[0]].Opponents[p]++;
            oa.Players[oa.Court.Team1.Members[1]].Opponents[p]++;
            var pter3 = oa.Players[oa.Court.Team2.Members[0]];
            self.Partners[oa.Court.Team2.Members[0]]++;
            self.Opponents[oa.Court.Team1.Members[0]]++;
            self.Opponents[oa.Court.Team1.Members[1]]++;
            pter3.Partners[p]++;
            oa.Court.Team2.Members.Add(p);

            #region add court
            oa.Round.Courts.Add(new Court(oa.Court.Team1, oa.Court.Team2));
            oa.Court = new();
            if (oa.Round.Courts.Count == oa.MaxCt) {
                oa.Tour.Rounds.Add(oa.Round.Clone());
                oa.Round = new();
            }
            #endregion

            break;
        }
    }

    #endregion

}
