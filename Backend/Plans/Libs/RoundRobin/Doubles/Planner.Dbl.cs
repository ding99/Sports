﻿using CSharpFunctionalExtensions;

using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public Result<(Tour t, Player[] p, string mst)> CreateDbl(int persons, int games, bool dsp) {
        var result = Pair(persons, games, dsp);

        if (result.IsSuccess) {
            if (dsp) {
                log.Information("{d}", DTour(result.Value.oall.Tour, $"{persons}/{games}Games"));
                log.Information("{d}", DPlayers(result.Value.oall.Players));
            }
            return (result.Value.oall.Tour, result.Value.oall.Players, result.Value.mst);
        } else {
            log.Error("Failed to create double: {err}", result.Error);
            return Result.Failure<(Tour, Player[], string)>(result.Error);
        }
    }

    public Result<(Overall oall, string mst)> Pair(int persons, int games, bool dsp) {
        if (games % (persons * 4) > 0) {
            return Result.Failure<(Overall, string)>($"games {games} should be a multiple of persons {persons}!");
        }

        //var master = CreateMaster(persons, games, true);
        var master = CreateMaster(persons, games, false);

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
                // get min part
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

        if (dsp) {
            log.Information("List ({ct})  {list}", master.Count, string.Join(", ", master));
        }

        return (oa, mst);
    }

    #region chose

    public IEnumerable<Order> GetMinPlay(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    public IEnumerable<Order> GetMinOppo(IEnumerable<Order> list, Overall oa) {
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

    public IEnumerable<Order> GetMinPart(IEnumerable<Order> list, Overall oa) {
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

    #region orig

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

    public static IEnumerable<Order> GetMinOppoOrg(IEnumerable<Order> list, Player[] players, Court ct) {
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

}
