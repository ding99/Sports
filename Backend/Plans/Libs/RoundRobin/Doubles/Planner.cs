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

    public void Select084() {
        int persons = 8, games = 32, times = 30;

        log.Information("Round Robin double: players {p}, games {games}. times {times}", persons, games, times);
        Chose8_4(persons, games, times);
    }

    public void Select094() {
        int persons = 9, games = 36, times = 30;

        log.Information("Round Robin double: players {p}, games {games}. times {times}", persons, games, times);
        Chose9_4(persons, games, times);
    }

    public void Select104() {
        int persons = 10, games = 40, times = 600;

        log.Information("Round Robin double: players {p}, games {games}. times {times}", persons, games, times);
        Chose10_4(persons, games, times);
    }


    public void DisplaySamples(int persons, int games) {
        log.Information("Round Robin Samples: persons {persons}, games {games}", persons, games);

        //TODO
        ShowSample(persons);
    }

}