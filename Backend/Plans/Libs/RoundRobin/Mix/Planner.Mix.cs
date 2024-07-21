using CSharpFunctionalExtensions;

using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public void CreateMix(int men, int women, int games) {
        var result = Pair(men, women, games);

        if (result.IsSuccess) {
            log.Information(DTour(result.Value.tour, $"{men}M/{women}W-{games}Games"));
            log.Information("{dsp}", DPlayers(result.Value.players));
        } else {
            log.Error("{err}", result.Error);
        }
    }

    #region create tour

    public Result<(Tour tour, Player[] players)> Pair(int men, int women, int games) {

        if (men == 0 || women == 0) {
            return Result.Failure<(Tour, Player[])>("The number of both men and women players must not be 0.");
        }

        var master = CreateMaster(men, women, games);
        if (master.Men.Count != master.Women.Count) {
            return Result.Failure<(Tour, Player[])>($"The number({master.Men.Count}) of men players must be equal to the number({master.Women.Count}) of women players.");
        }

        var tour = new Tour();
        var oa = new Overall(men, women, games);
        log.Debug("Overrall: men {m} women {w} games {g} maxCourts {m}", oa.Men, oa.Women, oa.Games, oa.MaxCt);
        int count;

        while ((count = master.Men.Count) > 0) {
            var orgM = master.Men
                .Select((d, i) => new Order(i, d))
                .Where(x => !oa.Round.ContainsM(x.Person) && oa.Court.ContainM(x.Person));
            var orgW = master.Women
                .Select((d, i) => new Order(i, d))
                .Where(x => !oa.Round.ContainsW(x.Person) && oa.Court.ContainW(x.Person));

            var minPlayM = GetMinPlayed(orgM, oa.PlayerM);
            var minPlayW = GetMinPlayed(orgW, oa.PlayerW);
        }

        return (tour, oa.PlayerM);
    }

    #region create chose

    public IEnumerable<Order> GetMinPlayed(IEnumerable<Order> list, Player[] players) {
        var quali = players.Where(p => list.Any(o => o.Person == p.Self));
        var min = quali.Min(p => p.Played);
        var minPlayed = quali.Where(p => p.Played == min);
        var result = list.Where(o => minPlayed.Any(p => p.Self == o.Person));
        return result.Any() ? result : list;
    }

    #endregion

    #region update

    public bool UpdateList(Overall oa, Master master) {
        //var result = 
        return true;
    }

    #endregion

    #endregion

}
