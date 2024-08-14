namespace Libs.RoundRobin.Doubles.Models;

public class Overall
{
    public Tour Tour { get; set; }
    public Round Round { get; set; }
    public Court Court { get; set; }
    public int PlayersCount { get; set; }
    public int GamesCount { get; set; }
    public int MaxCt { get; set; }

    public Player[] Players { get; set; }
    public IEnumerable<Order> Orders { get; set; }

    public Overall(int playersCount, int games)
    {
        PlayersCount = playersCount;
        GamesCount = games;
        Tour = new();
        Round = new();
        Court = new();
        Players = Enumerable.Range(0, playersCount).Select(i => new Player() {
            Self = i,
            Partners = new int[playersCount],
            Opponents = new int[playersCount],
        }).ToArray();
        Orders = [];
        MaxCt = playersCount / 4;
    }

    public void CheckCourt()
    {
        if (Court.Players() == 4)
        {
            if (Round.Courts.Count == MaxCt)
            {
                Tour.Rounds.Add(Round.Clone());
                Round = new() { Courts = [Court.Clone()] };
            }
            else
            {
                Round.Courts.Add(Court);
            }
        }

        if (Round.Courts.Count > 0)
        {
            Tour.Rounds.Add(Round);
        }
    }

}

public class Tour
{
    public List<Round> Rounds { get; set; }

    public Tour() { Rounds = []; }
    public Tour(List<Round> rounds)
    {
        Rounds = rounds;
    }
}

public class Round
{
    public List<Court> Courts { get; set; }

    public Round() { Courts = []; }
    public Round(List<Court> courts)
    {
        Courts = courts;
    }

    public bool Contain(int player)
    {
        return Courts.Any(c => c.Contain(player));
    }

    public Round Clone()
    {
        return new Round { Courts = new(Courts) };
    }
}

public class Court
{
    public Team Team1 { get; set; }
    public Team Team2 { get; set; }

    public Court() { Team1 = new(); Team2 = new(); }
    public Court(Team team1, Team team2)
    {
        Team1 = team1;
        Team2 = team2;
    }

    public bool Contain(int player)
    {
        return Team1.Players.Contains(player) || Team2.Players.Contains(player);
    }

    public int Players()
    {
        return Team1.Players.Count + Team2.Players.Count;
    }

    public Court Clone()
    {
        return new Court
        {
            Team1 = new() { Players = new(Team1.Players) },
            Team2 = new() { Players = new(Team2.Players) }
        };
    }
}

public class Team
{
    public List<int> Players { set; get; }

    public Team() { Players = []; }
    public Team(List<int> players)
    {
        Players = players;
    }

    public int Members() {
        return Players.Count;
    }

    public bool Contain(int player)
    {
        return Players.Contains(player);
    }
}
