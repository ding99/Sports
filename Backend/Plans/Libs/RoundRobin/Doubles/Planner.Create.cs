﻿using System.Text;

namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    public void Create(int persons, int games) {
        var (new10, players) = Find(persons, games);

        var orig = DTour(new10, "New10");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(orig);

        StringBuilder b = new();
        b.AppendLine(string.Join("", players.Select(p => DSummary(p))));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(b.ToString());
    }


    public (Tour, Summary[]) Find(int MaxPern, int MaxAttd) {
        var list = CreateOrder(MaxPern, MaxAttd);
        StringBuilder b = new();

        b.AppendLine($"List:  {string.Join(", ", list)}");
        var g = list.GroupBy(x => x).ToList();
        b.AppendLine($"Group: {string.Join(", ", g.Select(x => x.Count()))}");

        var (tour, summaries) = CreateTour(MaxPern, MaxAttd, list);
        b.AppendLine($"Rounds {tour.Rounds.Count}");

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(b);
        return (tour, summaries);
    }

    #region create tour

    public List<int> CreateOrder(int maxPlayers, int maxAttd) {
        List<int> list = [];
        int a;
        Random rd = new();
        int maxPosition = maxPlayers * maxAttd;

        while (list.Count < maxPosition) {
            a = rd.Next(maxPlayers);
            if (list.Count(x => x == a) < maxAttd) {
                list.Add(a);
            }
        }

        Console.WriteLine($"Group size {list.Count}");
        return list;
    }

    public (Tour, Summary[]) CreateTour(int maxPers, int maxAttd, List<int> list) {
        Tour tour = new();
        Round crtRd = new();
        Court crtCt = new();
        var players = Enumerable.Range(0, maxPers).Select(i => new Summary() {
            Self = i,
            Played = 0,
            Partners = new int[maxPers],
            Opponents = new int[maxPers]
        }).ToArray();

        int maxCt = maxPers / 4;
        Console.WriteLine($"MaxCourt {maxCt}");
        StringBuilder b = new();

        int i = 0, count;
        while ((count = list.Count) > 0) {
            for (i = 0; i < list.Count; i++) {
                if (!InRound(crtRd, list[i]) && !InCourt(crtCt, list[i]) && !Parted(players, crtCt, list[i])) {
                    (tour, players, crtRd, crtCt) = AppendPlayer(tour, players, crtRd, crtCt, maxCt, list[i]);
                    players.First(x => x.Self == list[i]).Played++;
                    b.Append($" {list[i]},");
                    list.RemoveAt(i);
                    break;
                }
            }

            if (list.Count == count) {
                break;
            }
        }

        if (CountPos(crtCt) > 0) {
            tour = AppendCt(tour, crtRd, crtCt, maxCt);
        }

        Console.WriteLine($"Added {b} ({b.ToString().Split(',').Length})");
        return (tour, players.ToArray());
    }

    private bool Parted(Summary[] players, Court ct, int p) {
        return ct.Team1.Players.Count == 1 && players.First(x => x.Self == ct.Team1.Players[0]).Partners[p] > 0
            || ct.Team2.Players.Count == 1 && players.First(x => x.Self == ct.Team2.Players[0]).Partners[p] > 0;
    }

    private bool InRound(Round rd, int p) {
        return rd.Courts.Any(x => InCourt(x, p));
    }

    private bool InCourt(Court ct, int p) {
        return ct.Team1.Players.Contains(p) || ct.Team2.Players.Contains(p);
    }

    private (Tour, Summary[], Round, Court) AppendPlayer(Tour tr, Summary[] players, Round rd, Court ct, int maxCt, int p) {
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

    private Round CloneRd(Round rd) {
        return new Round {
            Courts = new(rd.Courts)
        };
    }

    private Court CloneCt(Court ct) {
        return new Court {
            Team1 = new() { Players = new(ct.Team1.Players) },
            Team2 = new() { Players = new(ct.Team2.Players) }
        };
    }

    private int CountPos(Court court) {
        return court.Team1.Players.Count + court.Team2.Players.Count;
    }

    #endregion

}
