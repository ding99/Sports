using CSharpFunctionalExtensions;
using Serilog;

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

        var master = CreateMaster(persons, games);

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

        //var (tour, players) = CreateTour(persons, master);

        //return (tour, players);

        return oa;
    }

    #region create tour

    #region tour

    public (Tour, Player[]) CreateTour(int maxPlayers, List<int> list, Overall oa) {
        //Overall oa = new(maxPlayers / 4);
        var players = Enumerable.Range(0, maxPlayers).Select(i => new Player() {
            Self = i,
            Played = 0,
            Partners = new int[maxPlayers],
            Opponents = new int[maxPlayers]
        }).ToArray();
        int count;

        log.Information("Added");
        while ((count = list.Count) > 0) {
            var unset = list
                .Select((data, index) => new Order(index, data))
                .Where(x => !oa.Round.Contain(x.Person) && !oa.Court.Contain(x.Person));

            // min opponent
            unset = GetMinOppo(unset, players, oa.Court);
            log.Debug($" Oppo {count}", unset.Count());

            // min part
            unset = GetMinParted(unset, players, oa.Court);
            log.Debug($" Part {count}", unset.Count());

            // min play
            unset = GetMinPlayed(unset, players);
            log.Debug($" Play {count}", unset.Count());

            // min+1 parted
            //Console.ForegroundColor = ConsoleColor.White;
            //unset = GetMinPlusParted(unset, players, oa.Court);
            //Console.Write($" Plus {unset.Count()}");

            log.Debug(" rounds ({rs}) courts ({cs}) players ({ps})", oa.Tour.Rounds.Count, oa.Round.Courts.Count, oa.Court.Players());
            if (UpdateListOrg(oa, players, unset, list)) {
                continue;
            }

            if (list.Count == count) {
                break;
            }
        }

        if (list.Count == 1 && (oa.Court.Players() & 1) == 1) {
            AddPlayerOrg(oa, players, list.First());
        }

        if (oa.Court.Players() > 0 || oa.Round.Courts.Count > 0) {
            oa.CheckCourt();
        }

        log.Debug("Courts {xax}", oa.MaxCt);

        return (oa.Tour, players);
    }

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
        var psn = ct.Players() == 1 ? ct.Team1.Players[0] : ct.Team2.Players[0];
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
        var min = quali.Min(p => ct.Team1.Players.Min(c => p.Opponents[c]));
        var minPlayers = quali.Where(p => ct.Team1.Players.Any(c => p.Opponents[c] == min));
        var result = list.Where(i => minPlayers.Any(p => p.Self == i.Person));
        return result.Any() ? result : list;
    }

    #region remove
    public IEnumerable<Order> GetMinPlusParted(IEnumerable<Order> list, Player[] players, Court ct) {
        if ((ct.Players() & 1) == 0) {
            return list;
        }
        var psn = ct.Players() == 1 ? ct.Team1.Players[0] : ct.Team2.Players[0];
        var quali = players.Where(p => list.Any(s => s.Person == p.Self));
        var min = quali.Where(p => p.Self != psn).Min(p => p.Partners[psn]);
        var minPLayers = quali.Where(p => p.Self != psn && p.Partners[psn] == min);
        var selected = list.Where(i => minPLayers.Any(p => p.Self == i.Person));

        var minPlusPLayers = players.Where(p => p.Self != psn && list.Any(s => s.Person == p.Self) && p.Partners[psn] == min + 1);
        selected = selected.Concat(list.Where(i => minPlusPLayers.Any(p => p.Self == i.Person)));

        return selected.Any() ? selected : list;
    }
    #endregion

    #endregion

    public void UpdateList(Overall oa, List<int> master, IEnumerable<Order> list) {
        var min = list.First();

        if (min != null) {
            AddPlayer(oa, min.Person);
            master.RemoveAt(min.Index);
        }

        //var result = orders?.Count() > 0;
        //var groups = orders?.GroupBy(
        //    o => o.Person,
        //    (baseO, os) => new {
        //        Key = baseO,
        //        Count = os.Count()
        //    });
        //var max = groups?.Max(g => g.Count);
        //var group = groups?.First(g => g.Count == max);
        //var last = orders?.Last(o => o.Person == group?.Key);

        //if (result is true) {
        //    AddPlayer(oa, players, last!.Person);
        //    list.RemoveAt(last.Index);
        //    Log.Debug(" {person},", last.Person);
        //}

        //return result;
    }

    private static void AddPlayer(Overall oa, int p) {
        var self = oa.Players[p];
        self.Played++;
        switch (oa.Court.Players()) {
        case 0:
            oa.Court.Team1.Players.Add(p);
            break;
        case 1:
            var part1 = oa.Players[oa.Court.Team1.Players[0]];
            self.Partners[oa.Court.Team1.Players[0]]++;
            part1.Partners[p]++;
            oa.Court.Team1.Players.Add(p);
            break;
        case 2:
            oa.Players[oa.Court.Team1.Players[0]].Opponents[p]++;
            oa.Players[oa.Court.Team1.Players[1]].Opponents[p]++;
            self.Opponents[oa.Court.Team1.Players[0]]++;
            self.Opponents[oa.Court.Team1.Players[1]]++;
            oa.Court.Team2.Players.Add(p);
            break;
        case 3:
            oa.Players[oa.Court.Team1.Players[0]].Opponents[p]++;
            oa.Players[oa.Court.Team1.Players[1]].Opponents[p]++;
            var pter3 = oa.Players[oa.Court.Team2.Players[0]];
            self.Partners[oa.Court.Team2.Players[0]]++;
            self.Opponents[oa.Court.Team1.Players[0]]++;
            self.Opponents[oa.Court.Team1.Players[1]]++;
            pter3.Partners[p]++;
            oa.Court.Team2.Players.Add(p);

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

    #region remove

    public static bool UpdateListOrg(Overall oa, Player[] players, IEnumerable<Order>? orders, List<int> list) {
        var result = orders?.Count() > 0;
        var groups = orders?.GroupBy(
            o => o.Person,
            (baseO, os) => new {
                Key = baseO,
                Count = os.Count()
            });
        var max = groups?.Max(g => g.Count);
        var group = groups?.First(g => g.Count == max);
        var last = orders?.Last(o => o.Person == group?.Key);

        if (result is true) {
            AddPlayerOrg(oa, players, last!.Person);
            list.RemoveAt(last.Index);
            Log.Debug(" {person},", last.Person);
        }

        return result;
    }

    private static void AddPlayerOrg(Overall oa, Player[] players, int p) {
        players[p].Played++;
        switch (oa.Court.Players()) {
        case 0:
            oa.Court.Team1.Players.Add(p);
            break;
        case 1:
            var self1 = players[p];
            var pter1 = players[oa.Court.Team1.Players[0]];
            self1.Partners[oa.Court.Team1.Players[0]]++;
            pter1.Partners[p]++;
            oa.Court.Team1.Players.Add(p);
            break;
        case 2:
            players[oa.Court.Team1.Players[0]].Opponents[p]++;
            players[oa.Court.Team1.Players[1]].Opponents[p]++;
            var self2 = players.First(x => x.Self == p);
            self2.Opponents[oa.Court.Team1.Players[0]]++;
            self2.Opponents[oa.Court.Team1.Players[1]]++;
            oa.Court.Team2.Players.Add(p);
            break;
        case 3:
            players[oa.Court.Team1.Players[0]].Opponents[p]++;
            players[oa.Court.Team1.Players[1]].Opponents[p]++;
            var self3 = players[p];
            var pter3 = players[oa.Court.Team2.Players[0]];
            self3.Partners[oa.Court.Team2.Players[0]]++;
            self3.Opponents[oa.Court.Team1.Players[0]]++;
            self3.Opponents[oa.Court.Team1.Players[1]]++;
            pter3.Partners[p]++;
            oa.Court.Team2.Players.Add(p);
            if (oa.Round.Courts.Count == oa.MaxCt) {
                oa.Tour.Rounds.Add(oa.Round.Clone());
                oa.Round = new() { Courts = [oa.Court.Clone()] };
            } else {
                oa.Round.Courts.Add(oa.Court.Clone());
                if (oa.Round.Courts.Count == oa.MaxCt) {
                    oa.Tour.Rounds.Add(oa.Round.Clone());
                    oa.Round = new();
                }
            }
            oa.Court = new();
            break;
        }
    }

    #endregion

    #endregion

    #endregion

}
