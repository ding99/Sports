﻿using System.CommandLine;

using Libs.RoundRobin.Mix;

namespace Apps.RoundRobin.PlanMix;

public class Caller {

    public void Start(string[] args) {

        var men = new Argument<int>(
            name: "men",
            description: "Input the number of men players."
            );
        var women = new Argument<int>(
            name: "women",
            description: "Input the number of women players."
            );
        var games = new Argument<int>(
            name: "games",
            description: "Input the number of total games."
            );

        var root = new RootCommand("Plan a double round robin");
        root.AddArgument(men);
        root.AddArgument(women);
        root.AddArgument(games);
        //root.SetHandler((m, w, g) => new Planner().StartMixed(m, w, g), men, women, games);
        //root.SetHandler((m, w, g) => new Planner().Select66(), men, women, games);
        root.SetHandler((m, w, g) => new Planner().Select65(), men, women, games);

        var result = root.Invoke(args);
        Console.WriteLine($"Result: {result}");
    }

}
