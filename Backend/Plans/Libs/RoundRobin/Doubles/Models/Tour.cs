﻿namespace Libs.RoundRobin.Doubles.Models;

public class Overall {
    public Tour Tour { get; set; }
    public Round Round { get; set; }
    public Court Court { get; set; }
    public int PlayersCount { get; set; }
    public int GamesCount { get; set; }
    public int MaxCt { get; set; }

    public Player[] Players { get; set; }
    public IEnumerable<Order> Orders { get; set; }

    public Overall(int playersCount, int games) {
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

    public Round Clone() {
        return new Round { Courts = new(Courts) };
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
        return Team1.Members.Contains(player) || Team2.Members.Contains(player);
    }

    public int Players() {
        return Team1.Players() + Team2.Players();
    }

    public Court Clone() {
        return new Court {
            Team1 = new() { Members = new(Team1.Members) },
            Team2 = new() { Members = new(Team2.Members) }
        };
    }
}

public class Team {
    public List<int> Members { set; get; }

    public Team() { Members = []; }
    public Team(List<int> members) {
        Members = members;
    }

    public int Players() {
        return Members.Count;
    }

    public bool Contain(int player) {
        return Members.Contains(player);
    }
}
