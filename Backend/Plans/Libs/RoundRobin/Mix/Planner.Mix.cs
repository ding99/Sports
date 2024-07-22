using CSharpFunctionalExtensions;

using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public void CreateMix(int men, int women, int games) {
        var result = Pair(men, women, games);

        if (result.IsSuccess) {
            log.Information("{d}", DTour(result.Value.Tour, $"{men}M/{women}W-{games}Games"));
            log.Information("{d}", DPlayers(result.Value.PlayerM));
        } else {
            log.Error("{err}", result.Error);
        }
    }

    #region create tour

    public Result<Overall> Pair(int men, int women, int games) {

        if (men == 0 || women == 0) {
            return Result.Failure<Overall>("The number of both men and women players must not be 0.");
        }

        var master = CreateMaster(men, women, games);
        if (master.Men.Count != master.Women.Count) {
            return Result.Failure<Overall>($"The number({master.Men.Count}) of men players must be equal to the number({master.Women.Count}) of women players.");
        }

        var oa = new Overall(men, women, games);
        log.Debug("Overall: men {m} women {w} games {g} maxCourts {max}", oa.Men, oa.Women, oa.Games, oa.MaxCt);
        int count;

        while ((count = master.Men.Count) > 0) {
            var listM = master.Men
                .Select((d, i) => new Order(i, d))
                .Where(x => !oa.Round.ContainsM(x.Person) && !oa.Court.ContainM(x.Person));
            var listW = master.Women
                .Select((d, i) => new Order(i, d))
                .Where(x => !oa.Round.ContainsW(x.Person) && !oa.Court.ContainW(x.Person));

            listM = GetMinPlay(listM, oa.PlayerM);
            listW = GetMinPlay(listW, oa.PlayerW);

            //TODO update
            UpdateList(oa, master, listM, listW);
        }

        if (oa.Court.Players() > 0) {
            oa.Round.Courts.Add(oa.Court);
        }

        if (oa.Round.Courts.Count > 0) {
            oa.Tour.Rounds.Add(oa.Round);
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

    public IEnumerable<Order> GetMinPart(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    public IEnumerable<Order> GetMinOppoSame(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    public IEnumerable<Order> GetMinOppoDiff(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    #endregion

    #region update list

    public void UpdateList(Overall oa, Master master, IEnumerable<Order> men, IEnumerable<Order> women) {
        //log.Information("- {mc} {m}", m.Count(), string.Join(",", m.Select(x => x.Person)));
        //log.Information("  {wc} {w}", w.Count(), string.Join(",", w.Select(x => x.Person)));
        //log.Debug("Master: M {m} W {w}", master.Men.Count, master.Women.Count);

        int? times = null;
        var minM = men.First();
        var minW = women.First();

        foreach (var m in men) {
            foreach (var w in women) {
                if (!times.HasValue || oa.PlayerM[m.Person].Partners[w.Person] < times) {
                    times = oa.PlayerM[m.Person].Partners[w.Person];
                    minM = m;
                    minW = w;
                }
            }
        }

        log.Debug("Rds {r} Rd {cn} Ct {ctn}, M({mi}-{m}) W({wi}-{w})", oa.Tour.Rounds.Count, oa.Round.Courts.Count, oa.Court.Players(), minM?.Index, minM?.Person, minW.Index, minW?.Person);

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
