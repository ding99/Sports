namespace Libs.RoundRobin.Mix.Models;

public class Overall
{
    public Tour Tour { get; set; }
    public Round Round { get; set; }
    public Court Court { get; set; }
    public int Men { get; set; }
    public int Women { get; set; }
    public int Games { get; set; }

    public Overall(int men, int women,int games)
    {
        Men = men;
        Women = women;
        Games = games;
        Tour = new();
        Round = new();
        Court = new();
    }

    public void CheckCourt()
    {
        if (Court.Players() == 4)
        {
            if (Round.Courts.Count == Games)
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

    public bool ContainsM(int player)
    {
        return Courts.Any(c => c.ContainM(player));
    }

    public bool ContainsW(int player) {
        return Courts.Any(c => c.ContainW(player));
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

    public bool ContainM(int player)
    {
        return Team1.ContainsM(player) || Team2.ContainsM(player);
    }

    public bool ContainW(int player) {
        return Team1.ContainsW(player) || Team2.ContainsW(player);
    }

    public int Players()
    {
        return Team1.Players() + Team2.Players();
    }

    public Court Clone()
    {
        return new Court
        {
            Team1 = new() { Man = Team1.Man, Woman = Team1.Woman },
            Team2 = new() { Man = Team2.Man, Woman = Team2.Woman },
        };
    }
}

public class Team
{
    public int Man { set; get; }
    public int Woman { set; get; }

    public Team() { Man = -1; Woman = -1; }

    public int Players()
    {
        return Man >= 0 ? 1 : 0 + Woman >= 0 ? 1 : 0;
    }

    public bool ContainsM(int m) {
        return Man == m;
    }

    public bool ContainsW(int w) {
        return Woman == w;
    }
}
