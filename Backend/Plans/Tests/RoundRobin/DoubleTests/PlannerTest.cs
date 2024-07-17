using System.Text;

using FluentAssertions;

using Libs.RoundRobin.Doubles;
using Libs.RoundRobin.Doubles.Models;

namespace Tests.RoundRobin.DoubleTests;

public class PlannerTest {

    private readonly Planner planner;

    public PlannerTest() {
        planner = new();
    }

    [Fact]
    public void UpdateListTest_Team1_Player0() {
        var oa = new Overall(1) {
            Tour = new() {
                Rounds = [
                new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
                new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
            ]
            },
            Round = new(),
            Court = new()
        };
        Player[] players = [
            new Player{ Self = 0, Played = 1, Partners = [0,0,0,0,0,1], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 1, Partners = [0,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        IEnumerable<Order> orders = [new(0, 3), new(1, 0), new(1, 4), new(3, 5), new(4, 1), new(5, 0), new(6, 2), new(7, 2), new(8, 5), new(9, 4)];
        List<int> list = [3, 0, 4, 5, 1, 0, 2, 2, 5, 4];
        StringBuilder b = new();

        var result = planner.UpdateList(oa, players, orders, list, b);

        result.Should().BeTrue();

        oa.Court.Team1.Players.Should().HaveCount(1);
        oa.Court.Team1.Players[0].Should().Be(0);
        oa.Court.Team2.Players.Should().HaveCount(0);

        players[0].Played.Should().Be(2);
        players[0].Partners[4].Should().Be(0);

        players[4].Played.Should().Be(1);
        players[4].Partners[0].Should().Be(0);

    }

    [Fact]
    public void UpdateListTest_Team1_Player1() {
        var oa = new Overall(1) {
            Tour = new() {
                Rounds = [
                new Round ([new Court(new Team([1, 2]), new Team([3, 4]))]),
                new Round ([new Court(new Team([5, 0]), new Team([1, 3]))])
            ]
            },
            Round = new(),
            Court = new() { Team1 = new([4]) }
        };
        Player[] players = [
            new Player{ Self = 0, Played = 1, Partners = [0,0,0,0,0,1], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 1, Played = 2, Partners = [0,0,1,1,0,0], Opponents = [1,0,0,1,1,1] },
            new Player{ Self = 2, Played = 1, Partners = [0,1,0,0,0,0], Opponents = [0,0,0,1,1,0] },
            new Player{ Self = 3, Played = 2, Partners = [0,1,0,0,1,0], Opponents = [1,1,1,0,0,1] },
            new Player{ Self = 4, Played = 2, Partners = [0,0,0,1,0,0], Opponents = [0,1,1,0,0,0] },
            new Player{ Self = 5, Played = 1, Partners = [1,0,0,0,0,0], Opponents = [0,1,0,1,0,0] }
        ];
        IEnumerable<Order> orders = [new(0, 3), new(1, 0), new(3, 5), new(4, 1), new(5, 0), new(6, 2), new(7, 2), new(8, 5)];
        List<int> list = [3, 0, 4, 5, 1, 0, 2, 2, 5];
        StringBuilder b = new();

        var result = planner.UpdateList(oa, players, orders, list, b);

        result.Should().BeTrue();

        oa.Court.Team1.Players.Should().HaveCount(2);
        oa.Court.Team1.Players[1].Should().Be(0);
        oa.Court.Team2.Players.Should().HaveCount(0);

        players[0].Played.Should().Be(2);
        players[0].Partners[4].Should().Be(1);

        players[4].Played.Should().Be(2);
        players[4].Partners[0].Should().Be(1);

    }

    [Fact]
    public void UpdateListTest_Team2_Player0() {
        //TODO
    }

    [Fact]
    public void UpdateListTest_Team2_Player1() {
        //TODO
    }

}