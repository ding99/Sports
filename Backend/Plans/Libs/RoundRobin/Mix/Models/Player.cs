namespace Libs.RoundRobin.Mix.Models;

public class Player {
    public int Self { get; set; }
    public int Played { get; set; }
    public int[] Partners { get; set; }
    public int[] OppoSame { get; set; }
    public int[] OppoDiff { get; set; }

    public Player() {
        Partners = [];
        OppoSame = [];
        OppoDiff = [];
    }
}

public class PlayerAdv {
    public int Self { get; set; }
    public int Played { get; set; }
    public Dictionary<int, int> Partners { get; set; }
    public Dictionary<int, int> OppoSame { get; set; }
    public Dictionary<int, int> OppoDiff { get; set; }

    public PlayerAdv() {
        Partners = [];
        OppoSame = [];
        OppoDiff = [];
    }
}

public class Order(int index, int person) {
    public int Index { get; set; } = index;
    public int Person { get; set; } = person;
}

public class Master {
    public List<int> Men { get; set; }
    public List<int> Women { get; set; }

    public Master() {
        Men = [];
        Women = [];
    }
}
