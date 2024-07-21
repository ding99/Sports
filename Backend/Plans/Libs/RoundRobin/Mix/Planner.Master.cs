using Libs.RoundRobin.Mix.Models;


namespace Libs.RoundRobin.Mix;

public partial class Planner {

    public Master CreateMaster(int men, int women, int games) {
        var master = new Master {
            Men = [3, 1, 2, 5, 0, 0, 5, 3, 1, 4, 5, 2, 5, 0, 2, 2, 2, 4, 1, 5, 2, 4, 0, 0, 5, 3, 0, 4, 1, 4, 4, 3, 1, 3, 1, 3],
            Women = [2, 2, 4, 1, 0, 1, 3, 0, 4, 5, 0, 3, 2, 5, 0, 1, 2, 3, 0, 4, 4, 5, 2, 2, 4, 5, 0, 3, 1, 5, 4, 3, 1, 1, 5, 3]
        };

        //int max = Math.Max(men, women) * games;
        //var master = new Master {
        //    Men = NewMaster(men, max),
        //    Women = NewMaster(women, max)
        //};

        log.Debug("Men   {men} [{ct}]", master.Men.Count, GroupMaster(master.Men));
        log.Debug("  {list}", string.Join(",", master.Men));
        log.Debug("Women {women} [{ct}]", master.Women.Count, GroupMaster(master.Women));
        log.Debug("  {list}", string.Join(",", master.Women));

        return master;
    }

    public string GroupMaster(List<int> players) {
        var groups = players.GroupBy(x => x, (k, g) => new { Key = k, Count = g.Count() }).OrderBy(x => x.Key);
        return string.Join(", ", groups.Select(x => $"{x.Key}-{x.Count}"));
    }

    public List<int> NewMaster(int players, int maxGames) {
        List<int> list = [];
        Random rd = new();
        int rounds = maxGames / players, n;

        while (list.Count < rounds * players) {
            n = rd.Next(players);
            if (list.Count(x => x == n) < rounds) {
                list.Add(n);
            }
        }

        while (list.Count < maxGames) {
            n = rd.Next(players);
            if (list.Count(x => x == n) < rounds + 1) {
                list.Add(n);
            }
        }

        return list;
    }

}
