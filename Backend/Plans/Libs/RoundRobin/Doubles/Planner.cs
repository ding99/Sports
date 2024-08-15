using Serilog;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    private readonly Serilog.Core.Logger log;

    public Planner() {
        log = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();
    }

    public void StartDouble(int persons, int games) {
        log.Information("Round Robin doubles: persons {persons}, games {games}", persons, games);

        CreateDbl(persons, games, true);
    }

    public void Select94() {
        int persons = 9, games = 36;
        int times = 50; // 200000;

        log.Information("Round Robin double: players {p}, games {games}. times {times}", persons, games, times);
        Chose94(persons, games, times);
    }



    public void DisplaySamples(int persons, int games) {
        log.Information("Round Robin Samples: persons {persons}, games {games}", persons, games);

        //TODO
        ShowSample(persons);
    }

}