using System.CommandLine;

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
            description: "Input the number of games one player needs to play."
            );

        var root = new RootCommand("Plan a double round robin");
        root.AddArgument(men);
        root.AddArgument(women);
        root.AddArgument(games);
        root.SetHandler((m, w, g) => new Planner().StartMixed(m, w, g), men, women, games);

        var result = root.Invoke(args);
        Console.WriteLine($"Result: {result}");
    }

}
