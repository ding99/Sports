using System.Collections.Generic;
using System.Text;

namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    public void CreateDbl(int persons, int games) {
        if ((persons * games) % 4 != 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"persons*games should be multiple of 4!");
            return;
        }

        var (tour, players, log) = Find(persons, games);
        Console.ResetColor();
        Console.WriteLine(log);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(DTour(tour, $"New_{persons}-{games}"));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(DPlayers(players));
    }


    public (Tour, List<Player>, string) Find(int maxPlayers, int maxGames) {
        var master = CreateMaster(maxPlayers, maxGames);

        StringBuilder b = new();

        b.AppendLine($"List ({master.Count})  {string.Join(", ", master)}");

        Console.WriteLine(b.ToString());

        var (tour, players, dsp) = CreateTour(maxPlayers, maxGames, master);
        b.Append(dsp);
        b.AppendLine($"Rounds {tour.Rounds.Count}");

        return (tour, players, b.ToString());
    }

    #region create tour

    #region master

    public static List<int> CreateMaster(int maxPlayers, int maxGames) {

        //last one
        return [1, 6, 1, 6, 5, 0, 5, 0, 0, 3, 2, 9, 7, 2, 6, 8, 0, 3, 9, 4, 3, 8, 2, 2, 5, 6, 4, 9, 8, 4, 1, 9, 6, 5, 6, 4, 0, 5, 7, 1, 9, 5, 8, 8, 3, 0, 2, 4, 3, 2, 1, 1, 3, 4, 8, 7, 9, 7, 7, 7];

        // 6 round
        return [3, 1, 6, 0, 8, 6, 3, 0, 5, 8, 9, 0, 0, 6, 3, 9, 9, 0, 1, 8, 1, 0, 7, 5, 6, 1, 6, 6, 9, 7, 5, 4, 1, 8, 8, 4, 9, 8, 4, 4, 9, 7, 1, 7, 3, 4, 4, 7, 3, 7, 5, 3, 2, 2, 5, 2, 2, 2, 2, 5];

        #region random
        List<int> list = [];
        Random rd = new();
        int a, maxPosition = maxPlayers * maxGames;

        while (list.Count < maxPosition) {
            a = rd.Next(maxPlayers);
            if (list.Count(x => x == a) < maxGames) {
                list.Add(a);
            }
        }

        return list;
        #endregion
    }

    #endregion

    #region tour

    public (Tour, List<Player>, string) CreateTour(int maxPlayers, int maxGames, List<int> list) {
        Overall oa = new();
        bool changed = false;
        var players = Enumerable.Range(0, maxPlayers).Select(i => new Player() {
            Self = i,
            Played = 0,
            Partners = new int[maxPlayers],
            Opponents = new int[maxPlayers]
        }).ToList();

        int maxCt = maxPlayers / 4, count;
        StringBuilder b = new("Added");

        while ((count = list.Count) > 0) {
            var unset = list
                .Select((d, i) => new Order(i, d))
                .Where(x => !oa.Round.Contain(x.Ply) && !oa.Court.Contain(x.Ply) && !Parted(players, oa.Court, x.Ply));

            //TODO exception sometimes
            //TODO parted < maxPart

            var choosen = GetMinPlayed(unset, players);

            (changed, oa, players, choosen, list) = UpdateList(oa, players, maxCt, choosen, list, b);

            if (changed) {
                //TODO
            }

            if (list.Count == count) {
                break;
            }
        }

        if (list.Count == 1 && (oa.Court.Team1.Players.Count == 1 || oa.Court.Team2.Players.Count == 1)) {
            (oa, players) = AppendPlayer(oa, players, maxCt, list.First());
        }

        if (oa.Court.CountPlayers() > 0 || oa.Round.Courts.Count > 0) {
            oa.CheckCourt(maxCt);
        }

        b.AppendLine($" ({b.ToString().Split(',').Length - 1})");
        b.AppendLine($"Courts {maxCt}");
        return (oa.Tour, players, b.ToString());
    }

    #region chose
    
    public IEnumerable<Order> GetMinPlayed(IEnumerable<Order> list, List<Player> players) {
        if (!list.Any()) {
            return list;
        }
        var min = players.Where(p => list.Any(s => s.Ply == p.Self)).Min(p => p.Played);
        var minPlayeds = players.Where(p => list.Any(s => s.Ply == p.Self) && p.Played == min);
        return list.Where(i => minPlayeds.Any(p => p.Self == i.Ply));
    }

    //TODO: correct
    public IEnumerable<Order> GetLestParted(IEnumerable<Order> list, List<Player> players, Court ct) {
        var playedLess = players.Where(p => list.Any(s => s.Ply == p.Self)).Min(p => p.Played);
        var lestPlayeds = players.Where(p => list.Any(s => s.Ply == p.Self) && p.Played == playedLess);
        return list.Where(i => lestPlayeds.Any(p => p.Self == i.Ply));
    }

    #endregion

    private bool Parted(List<Player> players, Court ct, int p) {
        return ct.Team1.Players.Count == 1 && players.First(x => x.Self == ct.Team1.Players[0]).Partners[p] > 0
            || ct.Team2.Players.Count == 1 && players.First(x => x.Self == ct.Team2.Players[0]).Partners[p] > 0;
    }

    private (bool, Overall, List<Player>, IEnumerable<Order>?, List<int>) UpdateList(Overall oa, List<Player> players, int maxCt, IEnumerable<Order>? orders, List<int> list, StringBuilder b) {
        var result = orders?.Count() > 0;
        var fst = orders?.FirstOrDefault();

        if(result is true) {
            (oa, players) = AppendPlayer(oa, players, maxCt, fst!.Ply);
            players.First(x => x.Self == fst.Ply).Played++;
            b.Append($" {fst.Ply},");
            list.RemoveAt(fst.Idx);
        }

        return (result, oa, players, orders, list);
    }

    private (Overall, List<Player>) AppendPlayer(Overall oa, List<Player> players, int maxCt, int p) {
        switch (oa.Court.CountPlayers()) {
        case 0:
            oa.Court.Team1.Players.Add(p);
            break;
        case 1:
            var self1 = players.First(x => x.Self == p);
            var pter1 = players.First(x => x.Self == oa.Court.Team1.Players[0]);
            self1.Partners[oa.Court.Team1.Players[0]]++;
            pter1.Partners[p]++;
            oa.Court.Team1.Players.Add(p);
            break;
        case 2:
            players.First(x => x.Self == oa.Court.Team1.Players[0]).Opponents[p]++;
            players.First(x => x.Self == oa.Court.Team1.Players[1]).Opponents[p]++;
            var self2 = players.First(x => x.Self == p);
            self2.Opponents[oa.Court.Team1.Players[0]]++;
            self2.Opponents[oa.Court.Team1.Players[1]]++;
            oa.Court.Team2.Players.Add(p);
            break;
        case 3:
            players.First(x => x.Self == oa.Court.Team1.Players[0]).Opponents[p]++;
            players.First(x => x.Self == oa.Court.Team1.Players[1]).Opponents[p]++;
            var self3 = players.First(x => x.Self == p);
            var pter3 = players.First(x => x.Self == oa.Court.Team2.Players[0]);
            self3.Partners[oa.Court.Team2.Players[0]]++;
            self3.Opponents[oa.Court.Team1.Players[0]]++;
            self3.Opponents[oa.Court.Team1.Players[1]]++;
            pter3.Partners[p]++;
            oa.Court.Team2.Players.Add(p);
            if (oa.Round.Courts.Count == maxCt) {
                oa.Tour.Rounds.Add(oa.Round.Clone());
                oa.Round = new() { Courts = [oa.Court.Clone()] };
            } else {
                oa.Round.Courts.Add(oa.Court.Clone());
                if (oa.Round.Courts.Count == maxCt) {
                    oa.Tour.Rounds.Add(oa.Round.Clone());
                    oa.Round = new();
                }
            }
            oa.Court = new();
            break;
        }

        return (oa, players);
    }

    #endregion

    #region util


    private Court CloneCt(Court ct) {
        return new Court {
            Team1 = new() { Players = new(ct.Team1.Players) },
            Team2 = new() { Players = new(ct.Team2.Players) }
        };
    }

    #endregion

    #endregion

}
