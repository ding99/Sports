namespace Libs.RoundRobin.Doubles;

public class Summary {
    public int Self { get; set; }
    public int Played { get; set; }
    public int[] Partners { get; set; }
    public int[] Opponents { get; set; }

    public Summary() {
        Partners = [];
        Opponents = [];
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
}

public class Court {
    public Team Team1 { get; set; }
    public Team Team2 { get; set; }

    public Court() { Team1 = new(); Team2 = new(); }
    public Court(Team team1, Team team2) {
        Team1 = team1;
        Team2 = team2;
    }
}

public class Team {
    public List<int> Players { set; get; }

    public Team() { Players = []; }
    public Team(List<int> players) {
        Players = players;
    }
}
