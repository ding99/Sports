using FluentAssertions;

using Libs.RoundRobin.Doubles;
using Libs.RoundRobin.Doubles.Models;


namespace Tests.RoundRobin.DoubleTests;

public class PlannerTest {

    private readonly Planner planner;

    public PlannerTest() {
        planner = new();
    }

    #region min played

    [Fact]
    public void GetMinPlayedTest_Team1_Player1() {
        Player[] players = [
            new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        IEnumerable<Order> list = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];

        var result = planner.GetMinPlay(list, players);

        result.Should().HaveCount(4);

        var people = result.ToArray();
        people[0].Person.Should().Be(5);
        people[1].Person.Should().Be(2);
        people[2].Person.Should().Be(2);
        people[3].Person.Should().Be(5);
    }

    #endregion

    #region min partners

    [Fact]
    public void GetMinPartTest_Team1_Player1() {
        /**
        0-5 vs 2-4
        0-4 vs 1-3
        2-3 vs 1-5
        4
        **/
        Court court = new() { Team1 = new([4]) };
        Player[] players = [
            new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,1,1,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,0,1,0,1], Opponents = [1,0,1,1,1,0] },
            new Player{ Self = 2, Played = 2, Partners = [0,0,0,1,1,0], Opponents = [1,1,0,0,0,2] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,1,0,0,0], Opponents = [1,1,0,0,1,1] },
            new Player{ Self = 4, Played = 2, Partners = [1,0,1,0,0,0], Opponents = [1,1,0,1,0,1] },
            new Player{ Self = 5, Played = 2, Partners = [1,1,0,0,0,0], Opponents = [0,0,2,1,1,0] }
        ];
        Overall oa = new(6, 24) { Players = players, Court = court };
        IEnumerable<Order> list = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];

        var result = planner.GetMinPart(list, oa);

        result.Should().HaveCount(4);

        var people = result.ToArray();
        people[0].Person.Should().Be(3);
        people[1].Person.Should().Be(5);
        people[2].Person.Should().Be(1);
        people[3].Person.Should().Be(5);
    }

    #endregion

    #region min opponent

    [Fact]
    public void GetMinOppoTest_Team2_Player0() {
        /**
        0-5 vs 2-4
        0-4 vs 1-3
        2-3 vs 1-5
        4
        **/
        Court court = new() { Team1 = new([4]) };
        Player[] players = [
            new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,1,1,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,0,1,0,1], Opponents = [1,0,1,1,1,0] },
            new Player{ Self = 2, Played = 2, Partners = [0,0,0,1,1,0], Opponents = [1,1,0,0,0,2] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,1,0,0,0], Opponents = [1,1,0,0,1,1] },
            new Player{ Self = 4, Played = 2, Partners = [1,0,1,0,0,0], Opponents = [1,1,0,1,0,1] },
            new Player{ Self = 5, Played = 2, Partners = [1,1,0,0,0,0], Opponents = [0,0,2,1,1,0] }
        ];
        Overall oa = new(6, 24) { Players = players, Court = court };
        IEnumerable<Order> list = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];

        var result = planner.GetMinOppo(list, oa);

        result.Should().HaveCount(6);

        var people = result.ToArray();
        people[0].Person.Should().Be(3);
        people[1].Person.Should().Be(5);
        people[2].Person.Should().Be(1);
        people[3].Person.Should().Be(2);
        people[4].Person.Should().Be(2);
        people[5].Person.Should().Be(5);
    }

    #endregion

    #region update list

    [Fact]
    public void UpdateListTest_Team1_Player0() {
        /**
        1-2 vs 3-4
        5-0 vs 1-3
        **/
        Player[] players = [
            new Player{ Self = 0, Played = 1, Partners = [0,0,0,0,0,1], Opponents = [0,1,0,1,0,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 1, Partners = [0,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        var oa = new Overall(6, 24) {
            Tour = new() {
                Rounds = [
                new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
                new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
                ]
            },
            Round = new(),
            Court = new(),
            Players = players
        };
        IEnumerable<Order> list = [new(0, 3), new(1, 0), new(1, 4), new(3, 5), new(4, 1), new(5, 0), new(6, 2), new(7, 2), new(8, 5), new(9, 4)];
        List<int> master = [3, 0, 4, 5, 1, 0, 2, 2, 5, 4];

        planner.UpdateList(oa, master, list);

        oa.Court.Team1.Members.Should().HaveCount(1);
        oa.Court.Team1.Members[0].Should().Be(3);
        oa.Court.Team2.Members.Should().HaveCount(0);

        oa.Players[0].Played.Should().Be(1);
        oa.Players[0].Partners[4].Should().Be(0);

        oa.Players[4].Played.Should().Be(1);
        oa.Players[4].Partners[0].Should().Be(0);
    }

    [Fact]
    public void UpdateListTest_Team1_Player1() {
        /**
        1-2 vs 3-4
        5-0 vs 1-3
        **/
        Player[] players = [
            new Player{ Self = 0, Played = 1, Partners = [0,0,0,0,0,1], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 2, Partners = [0,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        var oa = new Overall(6 ,24) {
            Tour = new() {
                Rounds = [
                new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
                new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
            ]
            },
            Round = new(),
            Court = new() { Team1 = new([4]) },
            Players = players
        };
        IEnumerable<Order> list = [new(0, 3), new(1, 0), new(3, 5), new(4, 1), new(5, 0), new(6, 2), new(7, 2), new(8, 5)];
        List<int> master = [3, 0, 4, 5, 1, 0, 2, 2, 5];

        planner.UpdateList(oa, master, list);

        oa.Court.Team1.Members.Should().HaveCount(2);
        oa.Court.Team1.Members[1].Should().Be(3);
        oa.Court.Team2.Members.Should().HaveCount(0);

        oa.Players[0].Played.Should().Be(1);
        oa.Players[0].Partners[4].Should().Be(0);

        oa.Players[4].Played.Should().Be(2);
        oa.Players[4].Partners[0].Should().Be(0);
    }

    [Fact]
    public void UpdateListTest_Team2_Player0() {
        /**
        1-2 vs 3-4
        5-0 vs 1-3
        **/
        Player[] players = [
            new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        var oa = new Overall(6, 24) {
            Tour = new() {
                Rounds = [
                new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
                new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
            ]
            },
            Round = new(),
            Court = new() { Team1 = new([4, 0]) },
            Players = players
        };
        IEnumerable<Order> list = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];
        List<int> master = [3, 4, 5, 1, 0, 2, 2, 5];

        planner.UpdateList(oa, master, list);

        oa.Court.Team1.Members.Should().HaveCount(2);
        oa.Court.Team2.Members.Should().HaveCount(1);
        oa.Court.Team2.Members[0].Should().Be(3);

        oa.Players[5].Played.Should().Be(1);
    }

    [Fact]
    public void UpdateListTest_Team2_Player1() {
        /**
        1-2 vs 3-4
        5-0 vs 1-3
        **/
        Player[] players = [
            new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 2, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        var oa = new Overall(6, 24) {
            Tour = new() {
                Rounds = [
                new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
                new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
            ]
            },
            Round = new(),
            Court = new() { Team1 = new([4, 0]), Team2 = new([5]) },
            Players = players
        };
        IEnumerable<Order> list = [new(0, 3), new(2, 1), new(4, 2), new(5, 2)];
        List<int> master = [3, 4, 1, 0, 2, 2, 5];

        planner.UpdateList(oa, master, list);

        oa.Tour.Rounds.Should().HaveCount(3);
        oa.Tour.Rounds[2].Courts[0].Team2.Members[1].Should().Be(3);

        oa.Round.Courts.Should().HaveCount(0);

        oa.Court.Team1.Members.Should().HaveCount(0);
        oa.Court.Team2.Members.Should().HaveCount(0);

        oa.Players[2].Played.Should().Be(1);
        oa.Players[2].Partners[5].Should().Be(0);
        oa.Players[2].Opponents[0].Should().Be(0);
        oa.Players[2].Opponents[4].Should().Be(1);

        oa.Players[5].Partners[3].Should().Be(1);
        oa.Players[0].Opponents[3].Should().Be(2);
        oa.Players[4].Opponents[3].Should().Be(1);
    }

    #endregion

}