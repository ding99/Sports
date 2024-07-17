namespace Libs.RoundRobin.Doubles.Models;

public class Player {
    public int Self { get; set; }
    public int Played { get; set; }
    public int[] Partners { get; set; }
    public int[] Opponents { get; set; }

    public Player() {
        Partners = [];
        Opponents = [];
    }
}

public class Order(int index, int person) {
    public int Index { get; set; } = index;
    public int Person { get; set; } = person;
}

