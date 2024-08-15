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

        //ShowSample(persons);
        CreateDbl(persons, games, true);
    }

}