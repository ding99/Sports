using CSharpFunctionalExtensions;

using Libs.RoundRobin.Mix.Models;


namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public Result<(Tour t, Player[] p)> CreateMix(int men, int women, int games, bool dsp = false) {
        var result = Pair(men, women, games);

        if (result.IsSuccess) {
            if (dsp) {
                log.Information("{d}", DTour(result.Value.Tour, $"{men}M/{women}W-{games}Games"));
                log.Information("{d}", DPlayers(result.Value.PlayerM));
            }
            return (result.Value.Tour, result.Value.PlayerM);
        } else {
            log.Error("Failed to create mix: {err}", result.Error);
            return Result.Failure<(Tour, Player[])>(result.Error);
        }
    }

    #region create tour

    public Result<Overall> Pair(int men, int women, int games) {

        if (games % men > 0 || games % women > 0) {
            return Result.Failure<Overall>("Invalid games value.");
        }

        var master = CreateMaster(men, women, games);
        if (master.Men.Count != master.Women.Count) {
            return Result.Failure<Overall>($"The number({master.Men.Count}) of men players must be equal to the number({master.Women.Count}) of women players.");
        }

        var oa = new Overall(men, women, games/men);
        int count;

        try {
            while ((count = master.Men.Count) > 0) {
                var listM = master.Men
                    .Select((d, i) => new Order(i, d))
                    .Where(x => !oa.Round.ContainsM(x.Person) && !oa.Court.ContainM(x.Person));
                var listW = master.Women
                    .Select((d, i) => new Order(i, d))
                    .Where(x => !oa.Round.ContainsW(x.Person) && !oa.Court.ContainW(x.Person));

                listM = GetMinPlay(listM, oa.PlayerM);
                listW = GetMinPlay(listW, oa.PlayerW);

                listM = GetMinOppo(oa, listM, true);
                listW = GetMinOppo(oa, listW, false);
                //log.Warning($"({listM.Count()} {listW.Count()})");

                (listM, listW) = GetMinPart(oa, listM, listW);

                UpdateList(oa, master, listM, listW);
            }

            if (oa.Court.Players() > 0) {
                oa.Round.Courts.Add(oa.Court);
            }

            if (oa.Round.Courts.Count > 0) {
                oa.Tour.Rounds.Add(oa.Round);
            }
        } catch (Exception e) {
            return Result.Failure<Overall>(e.Message);
        }

        return oa;
    }

    #region create chose

    public IEnumerable<Order> GetMinPlay(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    public (IEnumerable<Order> ms, IEnumerable<Order> ws) GetMinPart(Overall oa, IEnumerable<Order> men, IEnumerable<Order> women) {
        List<Order> minM = [];
        List<Order> minW = [];
        int? parts = null;

        foreach (var m in men) {
            foreach (var w in women) {
                if (!parts.HasValue || oa.PlayerM[m.Person].Partners[w.Person] < parts) {
                    parts = oa.PlayerM[m.Person].Partners[w.Person];
                    minM = [new Order(m.Index, m.Person)];
                    minW = [new Order(w.Index, w.Person)];
                } else if (oa.PlayerM[m.Person].Partners[w.Person] == parts) {
                    minM.Add(new Order(m.Index, m.Person));
                    minW.Add(new Order(w.Index, w.Person));
                }
            }
        }

        return (Filter(minM), Filter(minW));
    }

    public List<Order> Filter(List<Order> list) {
        return list.GroupBy(o => o.Index, (o, g) => g.First()).ToList();
    }

    public IEnumerable<Order> GetMinOppo(Overall oa, IEnumerable<Order> list, bool man) {
        if(oa.Court.Players() < 2) {
            return list;
        }

        Player[] sames = man ? oa.PlayerM : oa.PlayerW;
        Player[] diffs = man ? oa.PlayerW : oa.PlayerM;
        int same = man ? oa.Court.Team1.Man : oa.Court.Team1.Woman;
        int diff = man ? oa.Court.Team1.Woman : oa.Court.Team1.Man;

        List<Order> midd = [];
        int? ops = null;
        foreach (var o in list) {
            if (!ops.HasValue || sames[same].OppoSame[o.Person] + diffs[diff].OppoDiff[o.Person] < ops) {
                ops = sames[same].OppoSame[o.Person] + diffs[diff].OppoDiff[o.Person];
                midd = [new Order(o.Index, o.Person)];
            } else if (sames[same].OppoSame[o.Person] + diffs[diff].OppoDiff[o.Person] == ops) {
                midd.Add(new Order(o.Index, o.Person));
            }
        }

        List<Order> mins = [];
        ops = null;
        foreach (var o in midd) {
            if (!ops.HasValue || Math.Abs(sames[same].OppoSame[o.Person] - diffs[diff].OppoDiff[o.Person]) < ops) {
                ops = Math.Abs(sames[same].OppoSame[o.Person] - diffs[diff].OppoDiff[o.Person]);
                mins = [new Order(o.Index, o.Person)];
            } else if (Math.Abs(sames[same].OppoSame[o.Person] - diffs[diff].OppoDiff[o.Person]) == ops) {
                mins.Add(new Order(o.Index, o.Person));
            }
        }

        return mins;
    }
    #endregion

    #region update list

    public void UpdateList(Overall oa, Master master, IEnumerable<Order> men, IEnumerable<Order> women) {
        var minM = men.First();
        var minW = women.First();
        //log.Debug("({ms} {ws}) Rds {r} Rd {cn} Ct {ctn}, M({mi}-{m}) W({wi}-{w})", men.Count(), women.Count(), oa.Tour.Rounds.Count, oa.Round.Courts.Count, oa.Court.Players(), minM?.Index, minM?.Person, minW.Index, minW?.Person);

        if (minM != null) {
            AddMan(oa, minM.Person);
            master.Men.RemoveAt(minM.Index);
        }
        if (minW != null) {
            AddWoman(oa, minW.Person);
            master.Women.RemoveAt(minW.Index);
        }
    }

    public void AddMan(Overall oa, int p) {
        oa.PlayerM[p].Played++;
        if (oa.Court.Team1.Man == -1) {
            oa.Court.Team1.Man = p;
            if (oa.Court.Team1.Woman != -1) {
                oa.PlayerM[p].Partners[oa.Court.Team1.Woman]++;
                oa.PlayerW[oa.Court.Team1.Woman].Partners[p]++;
            }
        } else {
            oa.Court.Team2.Man = p;
            oa.PlayerM[oa.Court.Team1.Man].OppoSame[p]++;
            oa.PlayerM[p].OppoSame[oa.Court.Team1.Man]++;
            oa.PlayerW[oa.Court.Team1.Woman].OppoDiff[p]++;
            oa.PlayerM[p].OppoDiff[oa.Court.Team1.Woman]++;
            if (oa.Court.Team2.Woman != -1) {
                oa.PlayerM[p].Partners[oa.Court.Team2.Woman]++;
                oa.PlayerW[oa.Court.Team2.Woman].Partners[p]++;
                AddCourt(oa);
            }
        }
    }

    public void AddWoman(Overall oa, int p) {
        oa.PlayerW[p].Played++;
        if (oa.Court.Team1.Woman == -1) {
            oa.Court.Team1.Woman = p;
            if (oa.Court.Team1.Man != -1) {
                oa.PlayerW[p].Partners[oa.Court.Team1.Man]++;
                oa.PlayerM[oa.Court.Team1.Man].Partners[p]++;
            }
        } else {
            oa.Court.Team2.Woman = p;
            oa.PlayerW[oa.Court.Team1.Woman].OppoSame[p]++;
            oa.PlayerM[oa.Court.Team1.Man].OppoDiff[p]++;
            if (oa.Court.Team2.Man != -1) {
                oa.PlayerW[p].Partners[oa.Court.Team2.Man]++;
                oa.PlayerM[oa.Court.Team2.Man].Partners[p]++;
                AddCourt(oa);
            }
        }
    }

    public static void AddCourt(Overall oa) {
        oa.Round.Courts.Add(new Court(oa.Court.Team1, oa.Court.Team2));
        oa.Court = new();
        if (oa.Round.Courts.Count == oa.MaxCt) {
            oa.Tour.Rounds.Add(new Round(oa.Round.Courts));
            oa.Round = new();
        }
    }

    #endregion

    #endregion

}
