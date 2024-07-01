using System.Collections.Generic;
using System.Text;

namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    public void Create(int persons, int games) {
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
        Console.WriteLine(DSummary(players));
    }


    public (Tour, List<Summary>, string) Find(int maxPlayers, int maxGames) {
        var master = CreateMaster(maxPlayers, maxGames);

        StringBuilder b = new();

        b.AppendLine($"List  {string.Join(", ", master)} ({master.Count})");

        var (tour, summaries, dsp) = CreateTour(maxPlayers, maxGames, master);
        b.Append(dsp);
        b.AppendLine($"Rounds {tour.Rounds.Count}");

        return (tour, summaries, b.ToString());
    }

    #region create tour

    #region master

    public static List<int> CreateMaster(int maxPlayers, int maxGames) {
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
    }

    #endregion

    #region loop

    public (Tour, List<Summary>, string) CreateTour(int maxPlayers, int maxGames, List<int> list) {
        Tour tour = new();
        Round round = new();
        Court court = new();
        var players = Enumerable.Range(0, maxPlayers).Select(i => new Summary() {
            Self = i,
            Played = 0,
            Partners = new int[maxPlayers],
            Opponents = new int[maxPlayers]
        }).ToList();

        int maxCt = maxPlayers / 4;
        StringBuilder b = new("Added");

        int count, crt;
        while ((count = list.Count) > 0) {
            var unset = list
                .Select((d, i) => new Order(i, d))
                .Where(x => !InRound(round, x.Ply) && !InCourt(court, x.Ply) && !Parted(players, court, x.Ply));

            //TODO exception sometimes
            //TODO parted < maxPart

            var lestPlayeds = GetLestPlayed(unset, players);

            var fst = lestPlayeds.Count() > 0 ? lestPlayeds.First() : unset.First();
            if (fst != null) {
                (tour, players, round, court) = AppendPlayer(tour, players, round, court, maxCt, fst.Ply);
                players.First(x => x.Self == fst.Ply).Played++;
                b.Append($" {fst.Ply},");
                list.RemoveAt(fst.Idx);
            }

            if (list.Count == count) {
                break;
            }
        }

        if (CountPos(court) > 0 || round.Courts.Count > 0) {
            tour = AppendCt(tour, round, court, maxCt);
        }

        b.AppendLine($" ({(b.ToString().Split(',').Length - 1)})");
        b.AppendLine($"Courts {maxCt}");
        return (tour, players, b.ToString());
    }

    #region chose
    
    public IEnumerable<Order> GetLestPlayed(IEnumerable<Order> list, List<Summary> players) {
        if (list.Count() < 1) {
            return list;
        }
        var lest = players.Where(p => list.Any(s => s.Ply == p.Self)).Min(p => p.Played);
        var lestPlayeds = players.Where(p => list.Any(s => s.Ply == p.Self) && p.Played == lest);
        return list.Where(i => lestPlayeds.Any(p => p.Self == i.Ply));
    }

    //TODO: correct
    public IEnumerable<Order> GetLestParted(IEnumerable<Order> list, List<Summary> players, Court ct) {
        var playedLess = players.Where(p => list.Any(s => s.Ply == p.Self)).Min(p => p.Played);
        var lestPlayeds = players.Where(p => list.Any(s => s.Ply == p.Self) && p.Played == playedLess);
        return list.Where(i => lestPlayeds.Any(p => p.Self == i.Ply));
    }

    #endregion

    private bool Parted(List<Summary> players, Court ct, int p) {
        return ct.Team1.Players.Count == 1 && players.First(x => x.Self == ct.Team1.Players[0]).Partners[p] > 0
            || ct.Team2.Players.Count == 1 && players.First(x => x.Self == ct.Team2.Players[0]).Partners[p] > 0;
    }

    private bool InRound(Round rd, int p) {
        return rd.Courts.Any(x => InCourt(x, p));
    }

    private bool InCourt(Court ct, int p) {
        return ct.Team1.Players.Contains(p) || ct.Team2.Players.Contains(p);
    }

    private (Tour, List<Summary>, Round, Court) AppendPlayer(Tour tr, List<Summary> players, Round rd, Court ct, int maxCt, int p) {
        switch (CountPos(ct)) {
        case 0:
            ct.Team1.Players.Add(p);
            break;
        case 1:
            var self1 = players.First(x => x.Self == p);
            var pter1 = players.First(x => x.Self == ct.Team1.Players[0]);
            self1.Partners[ct.Team1.Players[0]]++;
            pter1.Partners[p]++;
            ct.Team1.Players.Add(p);
            break;
        case 2:
            players.First(x => x.Self == ct.Team1.Players[0]).Opponents[p]++;
            players.First(x => x.Self == ct.Team1.Players[1]).Opponents[p]++;
            var self2 = players.First(x => x.Self == p);
            self2.Opponents[ct.Team1.Players[0]]++;
            self2.Opponents[ct.Team1.Players[1]]++;
            ct.Team2.Players.Add(p);
            break;
        case 3:
            players.First(x => x.Self == ct.Team1.Players[0]).Opponents[p]++;
            players.First(x => x.Self == ct.Team1.Players[1]).Opponents[p]++;
            var self3 = players.First(x => x.Self == p);
            var pter3 = players.First(x => x.Self == ct.Team2.Players[0]);
            self3.Partners[ct.Team2.Players[0]]++;
            self3.Opponents[ct.Team1.Players[0]]++;
            self3.Opponents[ct.Team1.Players[1]]++;
            pter3.Partners[p]++;
            ct.Team2.Players.Add(p);
            if (rd.Courts.Count == maxCt) {
                tr.Rounds.Add(CloneRd(rd));
                rd = new() { Courts = [CloneCt(ct)] };
            } else {
                rd.Courts.Add(CloneCt(ct));
                if (rd.Courts.Count == maxCt) {
                    tr.Rounds.Add(CloneRd(rd));
                    rd = new();
                }
            }
            ct = new();
            break;
        }

        return (tr, players, rd, ct);
    }

    private Tour AppendCt(Tour tr, Round rd, Court ct, int maxCt) {
        if (CountPos(ct) == 4) {
            if (rd.Courts.Count == maxCt) {
                tr.Rounds.Add(CloneRd(rd));
                rd = new() { Courts = [CloneCt(ct)] };
            } else {
                rd.Courts.Add(CloneCt(ct));
            }
        }

        if (rd.Courts.Count > 0) {
            tr.Rounds.Add(rd);
        }

        return tr;
    }

    private Tour AppendCt(Tour tour, Court court, int maxCt) {
        var lastR = tour.Rounds.LastOrDefault();
        if (lastR != null && lastR.Courts.Count < maxCt) {
            lastR.Courts.Add(CloneCt(court));
        } else {
            tour.Rounds.Add(new Round { Courts = [CloneCt(court)] });
        }
        return tour;
    }

    private int CountPos(Court court) {
        return court.Team1.Players.Count + court.Team2.Players.Count;
    }

    #endregion

    #region util

    private Round CloneRd(Round rd) {
        return new Round { Courts = new(rd.Courts) };
    }

    private Court CloneCt(Court ct) {
        return new Court {
            Team1 = new() { Players = new(ct.Team1.Players) },
            Team2 = new() { Players = new(ct.Team2.Players) }
        };
    }

    #endregion

    #endregion

}
