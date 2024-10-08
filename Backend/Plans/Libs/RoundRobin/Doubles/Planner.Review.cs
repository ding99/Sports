﻿using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public void ReviewTour(int persons, int games) {

        Tour? rr = null;

        switch (persons) {
        case 6:
            switch (games) {
            case 6:
                rr = GetSample0606();
                break;
            }
            break;
        case 8:
            switch (games) {
            case 4:
                rr = GetSample0804();
                break;
            }
            break;
        case 10:
            switch (games) {
            case 4:
                rr = GetSample104();
                break;
            case 6:
                rr = GetSample106();
                break;
            }
            break;
        }

        if (rr != null) {
            log.Information(DTour(rr, $"Sample{persons}"));
            log.Information(DPlayers(STour(rr)));
        } else {
            log.Error("Not found reviewed data for {p}-player, {g}-game!", persons, games);
        }
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
            return team.Members.Count(c => c == n);
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
            return s != n && team.Members.Contains(s) && team.Members.Contains(n) ? 1 : 0;
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
                return s != n && (court.Team1.Members.Contains(s) && court.Team2.Members.Contains(n) || court.Team1.Members.Contains(n) && court.Team2.Members.Contains(s)) ? 1 : 0;
            }

        }
    }
    #endregion

    #endregion

    #region max

    private static int MaxTour(Tour tour) {
        return tour.Rounds.Max(r => r.Courts.Max(c => MaxCourt(c)));

        static int MaxCourt(Court court) {
            return Math.Max(court.Team1.Members.Max(), court.Team2.Members.Max());
        }
    }

    #endregion

    #region data

    // 20240818 FMS B, 10-player 4-game
    private static Tour GetSample104() {
        return new Tour(new([
            new(new([
                new(new([2, 4]), new([7, 6])),
                new(new([8, 5]), new([3, 0]))
            ])),
            new(new([
                new(new([1, 9]), new([2, 6])),
                new(new([3, 5]), new([7, 4]))
            ])),
            new(new([
                new(new([0, 9]), new([1, 8])),
                new(new([6, 5]), new([2, 7]))
            ])),
            new(new([
                new(new([1, 4]), new([3, 0])),
                new(new([8, 9]), new([5, 7]))
            ])),
            new(new([
                new(new([6, 8]), new([1, 3])),
                new(new([2, 9]), new([4, 0]))
            ]))
        ]));
    }

    // 10-player 6-game
    private static Tour GetSample106() {
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

    // 20240818 FMS C, 8-player 4-game
    private static Tour GetSample0804() {
        return new Tour(new([
            new(new([
                new(new([7, 0]), new([5, 2])),
                new(new([3, 6]), new([1, 4]))
            ])),
            new(new([
                new(new([5, 3]), new([2, 6])),
                new(new([0, 1]), new([7, 4]))
            ])),
            new(new([
                new(new([2, 7]), new([6, 4])),
                new(new([5, 0]), new([3, 1]))
            ])),
            new(new([
                new(new([7, 3]), new([5, 4])),
                new(new([2, 0]), new([1, 6]))
            ]))
        ]));
    }

    // downloaded 6-player 6-game
    private static Tour GetSample0606() {
        return new Tour(new([
            new(new([
                new(new([0, 5]), new([1, 3]))
            ])),
            new(new([
                new(new([3, 4]), new([0, 2]))
            ])),
            new(new([
                new(new([2, 4]), new([1, 5]))
            ])),
            new(new([
                new(new([2, 5]), new([0, 1]))
            ])),
            new(new([
                new(new([0, 4]), new([3, 5]))
            ])),
            new(new([
                new(new([0, 3]), new([1, 2]))
            ])),
            new(new([
                new(new([3, 4]), new([1, 5]))
            ])),
            new(new([
                new(new([2, 3]), new([4, 5]))
            ])),
            new(new([
                new(new([1, 4]), new([0, 2]))
            ]))
        ]));
    }

    #endregion

}
