using CSharpFunctionalExtensions;

using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public Result<(Tour t, Player[] p)> CreateDbl(int persons, int games, bool dsp) {
        var result = Pair(persons, games, dsp);

        if (result.IsSuccess) {
            if (dsp) {
                log.Information("{d}", DTour(result.Value.Tour, $"{persons}/{games}Games"));
                log.Information("{d}", DPlayers(result.Value.Players));
            }
            return (result.Value.Tour, result.Value.Players);
        } else {
            log.Error("Failed to create double: {err}", result.Error);
            return Result.Failure<(Tour, Player[])>(result.Error);
        }
    }

    public Result<Overall> Pair(int persons, int games, bool dsp) {
        if (games % (persons * 4) > 0) {
            return Result.Failure<Overall>($"games {games} should be a multiple of persons {persons}!");
        }

        var master = CreateMaster(persons, games, true);

        var oa = new Overall(persons, games);
        int count;

        if (dsp) {
            log.Information("({c}) {d}", master.Count, string.Join(',', master));
            log.Information("{d}", DPlayers(oa.Players));
        }

        try {
            while ((count = master.Count) > 0) {

                var list = master
                    .Select((d, i) => new Order(i, d))
                    .Where(x => !oa.Round.Contain(x.Person) && !oa.Court.Contain(x.Person));

                log.Debug("  {count}", count);
                //TODO

                // get min play
                // get min oppo
                // get min part

                UpdateList(oa, master, list);
            }

            if (oa.Court.Players() > 0) {
                oa.Round.Courts.Add(oa.Court);
            }

            if (oa.Round.Courts.Count > 0) {
                oa.Tour.Rounds.Add(oa.Round);
            }

        } catch (Exception e) {
            return Result.Failure<Overall>(e.Message + e.StackTrace);
        }

        log.Information("List ({ct})  {list}", master.Count, string.Join(", ", master));

        return oa;
    }

    #region create tour

    #region chose
    
    public static IEnumerable<Order> GetMinPlayed(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(s => s.Person == p.Self));
        var min = quali.Min(p => p.Played);
        var minPlayeds = quali.Where(p => p.Played == min);
        var result = list.Where(i => minPlayeds.Any(p => p.Self == i.Person));
        return result.Any() ? result : list;
    }

    public static IEnumerable<Order> GetMinParted(IEnumerable<Order> list, Player[] players, Court ct) {
        if ((ct.Players() & 1) == 0) {
            return list;
        }
        var psn = ct.Players() == 1 ? ct.Team1.Members[0] : ct.Team2.Members[0];
        var quali = players.Where(p => list.Any(s => s.Person == p.Self));
        var min = quali.Min(p => p.Partners[psn]);
        var minPlayers = quali.Where(p => p.Partners[psn] == min);
        var result = list.Where(i => minPlayers.Any(p => p.Self == i.Person));
        return result.Any() ? result : list;
    }

    public static IEnumerable<Order> GetMinOppo(IEnumerable<Order> list, Player[] players, Court ct) {
        if (ct.Players() < 2) {
            return list;
        }
        var quali = players.Where(p => list.Any(s => s.Person == p.Self));
        var min = quali.Min(p => ct.Team1.Members.Min(c => p.Opponents[c]));
        var minPlayers = quali.Where(p => ct.Team1.Members.Any(c => p.Opponents[c] == min));
        var result = list.Where(i => minPlayers.Any(p => p.Self == i.Person));
        return result.Any() ? result : list;
    }

    #endregion

    #region util

    public void UpdateList(Overall oa, List<int> master, IEnumerable<Order> list) {
        var min = list.First();
        if (min != null) {
            AddPlayer(oa, min.Person);
            master.RemoveAt(min.Index);
        }
    }

    private void AddPlayer(Overall oa, int p) {
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

    #endregion

}
