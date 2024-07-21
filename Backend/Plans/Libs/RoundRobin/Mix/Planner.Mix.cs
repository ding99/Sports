using System.Text;

using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public void CreateMix(int men, int women, int games) {

        Console.WriteLine("Create Mix");

        var (tour, players, log) = Pair(men, women, games);
        Console.ResetColor();
        Console.WriteLine(log);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(DTour(tour, $"{men}M/{women}W-{games}Games"));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(DPlayers(players));

    }

    public (Tour, Player[], string) Pair(int men, int women, int games) {
        var master = CreateMaster(men, women, games);

        StringBuilder b = new();

        b.AppendLine($"Men List ({master.Men.Count}) {string.Join(", ", master.Men)}");
        b.AppendLine($"Women List ({master.Women.Count}) {string.Join(", ", master.Women)}");

        var (tour, players, dsp) = CreateTour(men, women, games, master);
        b.Append(dsp);

        return (tour, players, b.ToString());
    }

    #region create tour

    public (Tour, Player[], string) CreateTour(int men, int women, int games, Master master) {
        var oa = new Overall(men, women, games);
        Tour tour = new();
        StringBuilder b = new();
        var players = Enumerable.Range(0, men).Select(i => new Player() {
            Self = i,
            OppoM = new int[men],
            OppoW = new int[women]
        }).ToArray();
        int count;

        //TODO

        return (tour, players, b.ToString());
    }

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
