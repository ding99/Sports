namespace Libs.RoundRobin.Doubles;

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


public class Overall {
    public Tour Tour { get; set; }
    public Round Round { get; set; }
    public Court Court { get; set; }

    public Overall() {
        Tour = new();
        Round = new();
        Court = new();
    }
}

public class Tour {
    public List<Round> Rounds { get; set; }

    public Tour() { Rounds = []; }
    public Tour(List<Round> rounds) {
        Rounds = rounds;
    }
}

public class Round {
    public List<Court> Courts { get; set; }

    public Round() { Courts = []; }
    public Round(List<Court> courts) {
        Courts = courts;
    }

    public bool Contain(int player) {
        return Courts.Any(c => c.Contain(player));
    }
}

public class Court {
    public Team Team1 { get; set; }
    public Team Team2 { get; set; }

    public Court() { Team1 = new(); Team2 = new(); }
    public Court(Team team1, Team team2) {
        Team1 = team1;
        Team2 = team2;
    }

    public bool Contain(int player) {
        return Team1.Players.Contains(player) || Team2.Players.Contains(player);
    }
}

public class Team {
    public List<int> Players { set; get; }

    public Team() { Players = []; }
    public Team(List<int> players) {
        Players = players;
    }

    public bool Contain(int player) {
        return Players.Contains(player);
    }
}

public class Order(int idx, int ply) {
    public int Idx { get; set; } = idx;
    public int Ply { get; set; } = ply;
}
