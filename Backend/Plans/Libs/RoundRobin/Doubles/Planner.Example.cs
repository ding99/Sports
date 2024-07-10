namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    public static void ShowSample(int persons) {

        Tour? rr;

        switch (persons) {
        case 10:
            rr = GetSample10();
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"UnSupported players number {persons}!");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(DTour(rr, $"Sample{persons}"));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(DPlayers(STour(rr)));
    }


    #region statistics

    private static Player[] STour(Tour tour) {
        var max = MaxTour(tour);
        return Enumerable.Range(0, max + 1).Select(i => new Player() {
            Self = i,
            Played = PlayedTour(tour, i),
            Partners = PartsTour(tour, i, max + 1),
            Opponents = OpposTour(tour, i, max + 1)
        }).ToArray();
    }

    #region played
    private static int PlayedTour(Tour tour, int n) {
        return tour.Rounds.Sum(r => r.Courts.Sum(c => PlayedCourt(c, n)));

        int PlayedCourt(Court court, int n) {
            return PlayedTeam(court.Team1, n) + PlayedTeam(court.Team2, n);
        }

        int PlayedTeam(Team team, int n) {
            return team.Players.Count(c => c == n);
        }
    }
    #endregion

    #region partner
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
        
        int PartTeam(Team team, int s, int n) {
            return s != n && team.Players.Contains(s) && team.Players.Contains(n) ? 1 : 0;
        }
    }
    #endregion

    #region opponent
    private static int[] OpposTour(Tour tour, int s, int mx) {
        return Enumerable.Range(0, mx).Select(i => OppoTour(tour, s, i)).ToArray();

        int OppoTour(Tour tour, int s, int n) {
            return tour.Rounds.Sum(r => OppoRound(r, s, n));

            int OppoRound(Round round, int s, int n) {
                return round.Courts.Sum(c => OppoCourt(c, s, n));
            }

            int OppoCourt(Court court, int s, int n) {
                return s != n && (court.Team1.Players.Contains(s) && court.Team2.Players.Contains(n) || court.Team1.Players.Contains(n) && court.Team2.Players.Contains(s)) ? 1 : 0;
            }

        }
    }
    #endregion

    #endregion

    #region max

    private static int MaxTour(Tour tour) {
        return tour.Rounds.Max(r => r.Courts.Max(c => MaxCourt(c)));

        static int MaxCourt(Court court) {
            return Math.Max(court.Team1.Players.Max(), court.Team2.Players.Max());
        }
    }

    #endregion

    #region data

    private static Tour GetSample10() {
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
