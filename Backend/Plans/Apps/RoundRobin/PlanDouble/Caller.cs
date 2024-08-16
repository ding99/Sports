using System.CommandLine;

using Libs.RoundRobin.Doubles;


namespace Apps.RoundRobin.PlanDouble;

public class Caller {

    public void Start(string[] args) {

        var players = new Argument<int>(
            name: "players",
            description: "Input the number of players."
            );
        var games = new Argument<int>(
            name: "games",
            description: "Input the number of total games."
            );

        var root = new RootCommand("Plan a double round robin");
        root.AddArgument(players);
        root.AddArgument(games);
        //root.SetHandler((p, g) => new Planner().StartDouble(p, g), players, games);
        //root.SetHandler((p, g) => new Planner().Select084(), players, games);
        root.SetHandler((p, g) => new Planner().Select094(), players, games);
        //root.SetHandler((p, g) => new Planner().Select104(), players, games);

        var result = root.Invoke(args);
        Console.WriteLine($"Result: {result}");
    }

}
