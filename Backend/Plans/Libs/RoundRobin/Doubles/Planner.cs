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

    public void Select054() {
        int persons = 5, games = 20, times = 5, max = 21;
        Chose(persons, games, times, max);
    }

    public void Select064() {
        int persons = 6, games = 24, times = 30, max = 19;
        Chose(persons, games, times, max);
    }

    public void Select074() {
        int persons = 7, games = 28, times = 30, max = 15;
        Chose(persons, games, times, max);
    }

    public void Select084() {
        int persons = 8, games = 32, times = 30, max = 9;
        Chose(persons, games, times, max);
    }

    public void Select094() {
        int persons = 9, games = 36, times = 100, max = 11;
        Chose(persons, games, times, max);
    }

    public void Select104() {
        int persons = 10, games = 40, times = 600, max = 9;
        Chose(persons, games, times, max);
    }


    public void DisplaySamples(int persons, int games) {
        log.Information("Round Robin Samples: persons {persons}, games {games}", persons, games);

        //TODO
        ShowSample(persons);
    }

}