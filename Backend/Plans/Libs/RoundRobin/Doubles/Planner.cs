using System.Text;

namespace RoundRobin.Doubles;

public class Planner {

    public void Start() {
        Console.WriteLine("-- Round Robin Doubles");

        ShowNet();
        Create10();

        Console.ResetColor();
    }

    public void ShowNet() {
        var rr10 = GetNet10();

        var orig = DTour(rr10, "Net10");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(orig);

        var stat = STour(rr10);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(stat);
    }

    public void Create10() {
        var (new10, players) = Find(10, 6);

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

    public (Tour , Summary[]) CreateTour(int maxPers, int maxAttd, List<int> list) {
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

    #region statistics

    private string STour(Tour tour) {
        var mx = MaxTour(tour);
        StringBuilder b = new($"Max ({mx + 1})");
        b.AppendLine();

        var players = Enumerable.Range(0, mx + 1).Select(i => new Summary() {
            Self = i,
            Played = CountTour(tour, i),
            Partners = PartsTour(tour, i, mx + 1),
            Opponents = OpposTour(tour, i, mx + 1)

        });
        b.AppendLine(string.Join("", players.Select(p => DSummary(p))));

        return b.ToString();
    }

    private static string DSummary(Summary s) {
        StringBuilder b = new();
        b.AppendLine($"-- {s.Self + 1} ({s.Played})");
        b.Append("Partners  ");
        b.Append(string.Join(", ", s.Partners.Select((v, i) => $"{i + 1}-{v}")));
        b.AppendLine($" ({s.Partners.Sum(x => x)})");
        b.Append("Opponents ");
        b.Append(string.Join(", ", s.Opponents.Select((v, i) => $"{i + 1}-{v}")));
        b.AppendLine($" ({s.Opponents.Sum(x => x)})");
        return b.ToString();
    }

    private static int[] PartsTour(Tour tour, int s, int mx) {
        return Enumerable.Range(0, mx).Select(i => PartTour(tour, s, i)).ToArray();
    }

    private static int PartTour(Tour tour, int s, int n) {
        return tour.Rounds.Sum(r => PartRound(r, s, n));

        int PartRound(Round round, int s, int n) {
            return round.Courts.Sum(c => PartCourt(c, s, n));
        }
    }

    private static int PartCourt(Court court, int s, int n) {
        return PartTeam(court.Team1, s, n) + PartTeam(court.Team2, s, n);
    }

    private static int PartTeam(Team team, int s, int n) {
        return s != n && team.Players.Contains(s) && team.Players.Contains(n) ? 1 : 0;
    }

    private static int[] OpposTour(Tour tour, int s, int mx) {
        return Enumerable.Range(0, mx).Select(i => OppoTour(tour, s, i)).ToArray();
    }

    private static int OppoTour(Tour tour, int s, int n) {
        return tour.Rounds.Sum(r => OppoRound(r, s, n));

        int OppoRound(Round round, int s, int n) {
            return round.Courts.Sum(c => OppoCourt(c, s, n));
        }

        int OppoCourt(Court court, int s, int n) {
            return s != n && (court.Team1.Players.Contains(s) && court.Team2.Players.Contains(n) || court.Team1.Players.Contains(n) && court.Team2.Players.Contains(s)) ? 1 : 0;
        }

    }

    private static int CountTour(Tour tour, int n) {
        return tour.Rounds.Sum(r => CountRound(r, n));

        int CountRound(Round round, int n) {
            return round.Courts.Sum(c => CountCourt(c, n));
        }

        int CountCourt(Court court, int n) {
            return CountTeam(court.Team1, n) + CountTeam(court.Team2, n);
        }

        int CountTeam(Team team, int n) {
            return team.Players.Count(c => c == n);
        }
    }

    private static int MaxTour(Tour tour) {
        return tour.Rounds.Max(r => MaxRound(r));

        int MaxRound(Round round) {
            return round.Courts.Max(c => MaxCourt(c));
        }

        int MaxCourt(Court court) {
            return Math.Max(MaxTeam(court.Team1), MaxTeam(court.Team2));
        }

        int MaxTeam(Team team) {
            return team.Players.Max();
        }
    }

    #endregion

    #region basic

    private static string DTour(Tour tour, string name) {
        StringBuilder b = new($"-- Tour {name} (Rounds {tour.Rounds.Count})");
        b.AppendLine();
        b.AppendLine(string.Join(
            Environment.NewLine,
            tour.Rounds.Select(r => string.Join(", ", r.Courts.Select(c => DCourt(c))))
        ));
        return b.ToString();

        string DCourt(Court c) {
            return $"{DTeam(c.Team1)} {DTeam(c.Team2)}";
        }

        string DTeam(Team t) {
            return t.Players.Count > 1 ? $"{t.Players[0] + 1,2}-{t.Players[1] + 1,-2}" : t.Players[0].ToString();
        }

    }

    #endregion

    #region example

    private static Tour GetNet10() {
        return new Tour(new([
            new(new([
                new(new([0, 6]), new([5, 8])),
                new(new([9, 4]), new([3, 2]))
            ])),
            new(new([
                new(new([0, 2]), new([3, 6])),
                new(new([1, 7]), new([8, 4]))
            ])),
            new(new([
                new(new([7, 3]), new([6, 2])),
                new(new([9, 5]), new([1, 8]))
            ])),
            new(new([
                new(new([9, 6]), new([4, 5])),
                new(new([1, 0]), new([7, 2]))
            ])),
            new(new([
                new(new([1, 4]), new([0, 8])),
                new(new([5, 7]), new([3, 9]))
            ])),
            new(new([
                new(new([4, 6]), new([9, 1])),
                new(new([5, 2]), new([7, 8]))
            ])),
            new(new([
                new(new([3, 0]), new([1, 5])),
                new(new([9, 2]), new([6, 7]))
            ])),
            new(new([
                new(new([3, 8]), new([0, 4]))
            ]))
        ]));
    }

    #endregion

}