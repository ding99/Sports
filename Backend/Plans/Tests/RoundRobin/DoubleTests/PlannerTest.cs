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

    //[Fact]
    //public void GetMinPartTest_Team1_Player1() {
    //    Court ct = new() { Team1 = new([4]) };
    //    Player[] players = [
    //        new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
    //        new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
    //        new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
    //        new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
    //    ];
    //    Overall oa = new(6, 30) { Players = players };
    //    IEnumerable<Order> list = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];

    //    var result = planner.GetMinPart(list, oa);

    //    result.Should().HaveCount(5);

    //    var people = result.ToArray();
    //    people[0].Person.Should().Be(5);
    //    people[1].Person.Should().Be(1);
    //    people[2].Person.Should().Be(2);
    //    people[3].Person.Should().Be(2);
    //    people[4].Person.Should().Be(5);
    //}

    #endregion

    #region min opponent

    //[Fact]
    //public void GetMinOppoTest_Team2_Player0() {
    //    Court ct = new() { Team1 = new([4, 0]) };
    //    Player[] players = [
    //        new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
    //        new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
    //        new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
    //        new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
    //    ];
    //    Overall oa = new(6, 30) { Players = players };
    //    IEnumerable<Order> list = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];

    //    var result = planner.GetMinOppo(list, oa);

    //    result.Should().HaveCount(6);

    //    var people = result.ToArray();
    //    people[0].Person.Should().Be(3);
    //    people[1].Person.Should().Be(5);
    //    people[2].Person.Should().Be(2);
    //    people[3].Person.Should().Be(2);
    //    people[4].Person.Should().Be(5);
    //}

    #endregion

    #region update list

    //[Fact]
    //public void UpdateListTest_Team1_Player0() {
    //    var oa = new Overall(1, 1) {
    //        Tour = new() {
    //            Rounds = [
    //            new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
    //            new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
    //        ]
    //        },
    //        Round = new(),
    //        Court = new()
    //    };
    //    Player[] players = [
    //        new Player{ Self = 0, Played = 1, Partners = [0,0,0,0,0,1], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
    //        new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
    //        new Player{ Self = 4, Played = 1, Partners = [0,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
    //        new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
    //    ];
    //    IEnumerable<Order> orders = [new(0, 3), new(1, 0), new(1, 4), new(3, 5), new(4, 1), new(5, 0), new(6, 2), new(7, 2), new(8, 5), new(9, 4)];
    //    List<int> list = [3, 0, 4, 5, 1, 0, 2, 2, 5, 4];

    //    var result = Planner.UpdateListOrg(oa, players, orders, list);

    //    result.Should().BeTrue();

    //    oa.Court.Team1.Members.Should().HaveCount(1);
    //    oa.Court.Team1.Members[0].Should().Be(0);
    //    oa.Court.Team2.Members.Should().HaveCount(0);

    //    players[0].Played.Should().Be(2);
    //    players[0].Partners[4].Should().Be(0);

    //    players[4].Played.Should().Be(1);
    //    players[4].Partners[0].Should().Be(0);
    //}

    //[Fact]
    //public void UpdateListTest_Team1_Player1() {
    //    var oa = new Overall(1, 1) {
    //        Tour = new() {
    //            Rounds = [
    //            new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
    //            new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
    //        ]
    //        },
    //        Round = new(),
    //        Court = new() { Team1 = new([4]) }
    //    };
    //    Player[] players = [
    //        new Player{ Self = 0, Played = 1, Partners = [0,0,0,0,0,1], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
    //        new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
    //        new Player{ Self = 4, Played = 2, Partners = [0,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
    //        new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
    //    ];
    //    IEnumerable<Order> orders = [new(0, 3), new(1, 0), new(3, 5), new(4, 1), new(5, 0), new(6, 2), new(7, 2), new(8, 5)];
    //    List<int> list = [3, 0, 4, 5, 1, 0, 2, 2, 5];

    //    var result = Planner.UpdateListOrg(oa, players, orders, list);

    //    result.Should().BeTrue();

    //    oa.Court.Team1.Members.Should().HaveCount(2);
    //    oa.Court.Team1.Members[1].Should().Be(0);
    //    oa.Court.Team2.Members.Should().HaveCount(0);

    //    players[0].Played.Should().Be(2);
    //    players[0].Partners[4].Should().Be(1);

    //    players[4].Played.Should().Be(2);
    //    players[4].Partners[0].Should().Be(1);
    //}

    //[Fact]
    //public void UpdateListTest_Team2_Player0() {
    //    var oa = new Overall(1, 1) {
    //        Tour = new() {
    //            Rounds = [
    //            new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
    //            new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
    //        ]
    //        },
    //        Round = new(),
    //        Court = new() { Team1 = new([4, 0]) }
    //    };
    //    Player[] players = [
    //        new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
    //        new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
    //        new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
    //        new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
    //    ];
    //    IEnumerable<Order> orders = [new(0, 3), new(2, 5), new(3, 1), new(5, 2), new(6, 2), new(7, 5)];
    //    List<int> list = [3, 4, 5, 1, 0, 2, 2, 5];

    //    var result = Planner.UpdateListOrg(oa, players, orders, list);

    //    result.Should().BeTrue();

    //    oa.Court.Team1.Members.Should().HaveCount(2);
    //    oa.Court.Team2.Members.Should().HaveCount(1);
    //    oa.Court.Team2.Members[0].Should().Be(5);

    //    players[5].Played.Should().Be(2);
    //}

    //[Fact]
    //public void UpdateListTest_Team2_Player1() {
    //    var oa = new Overall(6, 6) {
    //        Tour = new() {
    //            Rounds = [
    //            new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
    //            new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
    //        ]
    //        },
    //        Round = new(),
    //        Court = new() { Team1 = new([4, 0]), Team2 = new([5]) }
    //    };
    //    Player[] players = [
    //        new Player{ Self = 0, Played = 2, Partners = [0,0,0,0,1,1], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
    //        new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
    //        new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
    //        new Player{ Self = 4, Played = 2, Partners = [1,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
    //        new Player{ Self = 5, Played = 2, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
    //    ];
    //    IEnumerable<Order> orders = [new(0, 3), new(2, 1), new(4, 2), new(5, 2)];
    //    List<int> list = [3, 4, 1, 0, 2, 2, 5];

    //    var result = Planner.UpdateListOrg(oa, players, orders, list);

    //    result.Should().BeTrue();

    //    oa.Tour.Rounds.Should().HaveCount(3);
    //    oa.Tour.Rounds[2].Courts[0].Team2.Members[1].Should().Be(2);

    //    oa.Round.Courts.Should().HaveCount(0);

    //    oa.Court.Team1.Members.Should().HaveCount(0);
    //    oa.Court.Team2.Members.Should().HaveCount(0);

    //    players[2].Played.Should().Be(2);
    //    players[2].Partners[5].Should().Be(1);
    //    players[2].Opponents[0].Should().Be(1);
    //    players[2].Opponents[4].Should().Be(2);

    //    players[5].Partners[2].Should().Be(1);
    //    players[0].Opponents[2].Should().Be(1);
    //    players[4].Opponents[2].Should().Be(2);
    //}

    #endregion

}