using System.CommandLine;

using Libs.RoundRobin.Mix;

namespace Apps.RoundRobin.PlanMix;

public class Caller {

    public void Start(string[] args) {

        var players = new Argument<int>(
            name: "players",
            description: "Input the number of players."
            );
        var games = new Argument<int>(
            name: "games",
            description: "Input the number of games one player needs to play."
            );

        var root = new RootCommand("Plan a double round robin");
        root.AddArgument(players);
        root.AddArgument(games);
        root.SetHandler((p, g) => new Planner().StartMixed(p, g), players, games);

        var result = root.Invoke(args);
        Console.WriteLine($"Result: {result}");
    }

}
