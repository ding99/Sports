using CSharpFunctionalExtensions;

using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public void CreateMix(int men, int women, int games) {
        var result = Pair(men, women, games);

        if (result.IsSuccess) {
            log.Information(DTour(result.Value.Tour, $"{men}M/{women}W-{games}Games"));
            log.Information("{dsp}", DPlayers(result.Value.PlayerM));
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

            listM = GetMinPlayed(listM, oa.PlayerM);
            listW = GetMinPlayed(listW, oa.PlayerW);

            //TODO update
            UpdateList(oa, master, listM, listW);
        }

        return oa;
    }

    #region create chose

    public IEnumerable<Order> GetMinPlayed(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali?.Min(p => p.Played);
        var minPlayed = quali?.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed != null && minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    #endregion

    #region update

    public bool UpdateList(Overall oa, Master master, IEnumerable<Order> m, IEnumerable<Order> w) {
        //log.Information("- {mc} {m}", m.Count(), string.Join(",", m.Select(x => x.Person)));
        //log.Information("  {wc} {w}", w.Count(), string.Join(",", w.Select(x => x.Person)));
        //log.Debug("Master: M {m} W {w}", master.Men.Count, master.Women.Count);

        var fstM = m.First();
        if (fstM != null) {
            AddMan(oa, fstM.Person);
            master.Men.RemoveAt(fstM.Index);
        }

        var fstW = w.First();
        log.Debug("Rds {r}; Rd {cn}; Ct {ctn}. M ({mi}-{m}) W ({wi}-{w})", oa.Tour.Rounds.Count, oa.Round.Courts.Count, oa.Court.Players(), fstM?.Index, fstM?.Person, fstW?.Index, fstW?.Person);
        if (fstW != null) {
            AddWoman(oa, fstW.Person);
            master.Women.RemoveAt(fstW.Index);
        }

        return fstM != null && fstW != null;
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

    public void AddCourt(Overall oa) {
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
