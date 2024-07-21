using System.Text;

using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public void CreateMix(int men, int women, int games) {
        var (tour, players) = Pair(men, women, games);
        log.Information(DTour(tour, $"{men}M/{women}W-{games}Games"));
        log.Information("{dsp}", DPlayers(players));
    }

    public (Tour, Player[]) Pair(int men, int women, int games) {
        var master = CreateMaster(men, women, games);

        log.Information("Men   List {men} {list}", master.Men.Count, string.Join(", ", master.Men));
        log.Information("Women List {women} {list}", master.Women.Count, string.Join(", ", master.Women));

        var oa = new Overall(men, women, games);
        Tour tour = new();
        var players = Enumerable.Range(0, men).Select(i => new Player() {
            Self = i,
            OppoM = new int[men],
            OppoW = new int[women]
        }).ToArray();

        return (tour, players);
    }

    #region create tour

    #endregion

    #region master

    public Master CreateMaster(int men, int women, int games) {
        return new Master {
            Men = [4, 1, 5, 5, 0, 3, 5, 2, 3, 2, 3, 0, 1, 5, 5, 2, 4, 5, 0, 3, 0, 0, 3, 4, 0, 4, 4, 4, 2, 3, 1, 1, 1, 1, 2, 2],
            Women = [0, 5, 1, 5, 4, 3, 3, 1, 1, 0, 2, 5, 5, 4, 2, 1, 3, 4, 3, 5, 3, 5, 4, 1, 4, 0, 4, 0, 3, 2, 2, 2, 1, 0, 2, 0]
        };

        return new Master {
            Men = NewMaster(men, games),
            Women = NewMaster(women, games)
        };

    }

    public List<int> NewMaster(int maxPlayers, int maxGames) {
        List<int> list = [];
        Random rd = new();
        int n, maxPosition = maxPlayers * maxGames;

        while (list.Count < maxPosition) {
            n = rd.Next(maxPlayers);
            if (list.Count(x => x == n) < maxGames) {
                list.Add(n);
            }
        }

        return list;
    }

    #endregion

}
