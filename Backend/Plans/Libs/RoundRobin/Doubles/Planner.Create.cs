using System.Collections.Immutable;
using System.Text;
using Libs.RoundRobin.Doubles.Models;

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

    public (Tour, Player[], string) Find(int maxPlayers, int maxGames) {
        var master = CreateMaster(maxPlayers, maxGames);

        StringBuilder b = new();

        b.AppendLine($"List ({master.Count})  {string.Join(", ", master)}");

        Console.WriteLine(b.ToString());

        var (tour, players, dsp) = CreateTour(maxPlayers, master);
        b.Append(dsp);

        return (tour, players, b.ToString());
    }

    #region create tour

    #region tour

    public (Tour, Player[], string) CreateTour(int maxPlayers, List<int> list) {
        Overall oa = new(maxPlayers / 4);
        var players = Enumerable.Range(0, maxPlayers).Select(i => new Player() {
            Self = i,
            Played = 0,
            Partners = new int[maxPlayers],
            Opponents = new int[maxPlayers]
        }).ToArray();
        int count;

        StringBuilder b = new("Added");
        while ((count = list.Count) > 0) {
            var unset = list
                .Select((data, index) => new Order(index, data))
                .Where(x => !oa.Round.Contain(x.Person) && !oa.Court.Contain(x.Person));

            // min opponent
            Console.ForegroundColor = ConsoleColor.White;
            unset = GetMinOppo(unset, players, oa.Court);
            Console.Write($" Oppo {unset.Count()}");

            // min part
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            unset = GetMinParted(unset, players, oa.Court);
            Console.Write($" Part {unset.Count()}");

            // min play
            Console.ForegroundColor = ConsoleColor.Yellow;
            unset = GetMinPlayed(unset, players);
            Console.Write($" Play {unset.Count()}");

            // min+1 parted
            //Console.ForegroundColor = ConsoleColor.White;
            //unset = GetMinPlusParted(unset, players, oa.Court);
            //Console.Write($" Plus {unset.Count()}");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($" rounds ({oa.Tour.Rounds.Count}) courts ({oa.Round.Courts.Count}) players ({oa.Court.Players()})");
            if (UpdateList(oa, players, unset, list, b)) {
                continue;
            }

            if (list.Count == count) {
                break;
            }
        }

        if (list.Count == 1 && (oa.Court.Players() & 1) == 1) {
            AddPlayer(oa, players, list.First());
        }

        if (oa.Court.Players() > 0 || oa.Round.Courts.Count > 0) {
            oa.CheckCourt();
        }

        b.AppendLine($" ({b.ToString().Split(',').Length - 1})");
        b.AppendLine($"Courts {oa.MaxCt}");

        return (oa.Tour, players, b.ToString());
    }

    #region chose
    
    public IEnumerable<Order> GetMinPlayed(IEnumerable<Order> list, Player[] players) {
        var min = players.Where(p => list.Any(s => s.Person == p.Self)).Min(p => p.Played);
        Console.Write($" m({min})");
        var minPlayeds = players.Where(p => list.Any(s => s.Person == p.Self) && p.Played == min);
        Console.Write($" mP({minPlayeds.Count()})");
        var result = list.Where(i => minPlayeds.Any(p => p.Self == i.Person));
        return result != null && result.Any() ? result : list;
    }

    public IEnumerable<Order> GetMinParted(IEnumerable<Order> list, Player[] players, Court ct) {
        if((ct.Players() & 1) == 0) {
            return list;
        }
        var psn = ct.Players() == 1 ? ct.Team1.Players[0] : ct.Team2.Players[0];
        var min = players.Where(p => list.Any(s => s.Person == p.Self)).Min(p => p.Partners[psn]);
        var minPLayers = players.Where(p => p.Self != psn && list.Any(s => s.Person == p.Self) && p.Partners[psn] == min);
        var result = list.Where(i => minPLayers.Any(p => p.Self == i.Person));
        return result != null && result.Any() ? result : list;
    }

    public IEnumerable<Order> GetMinOppo(IEnumerable<Order> list, Player[] players, Court ct) {
        if (ct.Players() < 2) {
            return list;
        }
        var min = players.Where(p => list.Any(s => s.Person == p.Self)).Min(p => ct.Team1.Players.Min(c => p.Opponents[c]));
        var minPLayers = players.Where(p => list.Any(s => s.Person == p.Self) && ct.Team1.Players.Any(c => p.Opponents[c] == min));
        var result = list.Where(i => minPLayers.Any(p => p.Self == i.Person));
        return result != null && result.Any() ? result : list;
    }

    public IEnumerable<Order> GetMinPlusParted(IEnumerable<Order> list, Player[] players, Court ct) {
        if ((ct.Players() & 1) == 0) {
            return list;
        }
        var psn = ct.Players() == 1 ? ct.Team1.Players[0] : ct.Team2.Players[0];
        var min = players.Where(p => p.Self != psn && list.Any(s => s.Person == p.Self)).Min(p => p.Partners[psn]);
        var minPLayers = players.Where(p => p.Self != psn && list.Any(s => s.Person == p.Self) && p.Partners[psn] == min);
        var selected = list.Where(i => minPLayers.Any(p => p.Self == i.Person));

        var minPlusPLayers = players.Where(p => p.Self != psn && list.Any(s => s.Person == p.Self) && p.Partners[psn] == min + 1);
        selected = selected.Concat(list.Where(i => minPlusPLayers.Any(p => p.Self == i.Person)));

        return selected != null && selected.Any() ? selected : list;
    }

    #endregion

    private bool Parted(Player[] players, Court ct, int p) {
        return ct.Team1.Players.Count == 1 && players[ct.Team1.Players[0]].Partners[p] > 0
            || ct.Team2.Players.Count == 1 && players[ct.Team2.Players[0]].Partners[p] > 0;
    }

    private bool UpdateList(Overall oa, Player[] players, IEnumerable<Order>? orders, List<int> list, StringBuilder b) {
        var result = orders?.Count() > 0;
        var groups = orders?.GroupBy(
            o => o.Person,
            (baseO, os) => new {
                Key = baseO,
                Count = os.Count()
            });
        var max = groups?.Max(g => g.Count);
        var group = groups?.First(g => g.Count == max);
        var fst = orders?.First(o => o.Person == group?.Key);

        if (result is true) {
            AddPlayer(oa, players, fst!.Person);
            list.RemoveAt(fst.Index);
            b.Append($" {fst.Person},");
        }

        return result;
    }

    private void AddPlayer(Overall oa, Player[] players, int p) {
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

    #endregion

}
