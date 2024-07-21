﻿namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public void StartDouble(int persons, int games) {
        Console.WriteLine($"-- Round Robin Doubles: persons {persons}, games {games}");
        Console.WriteLine();

        //ShowSample(persons);
        CreateDbl(persons, games);

        Console.ResetColor();
    }

}