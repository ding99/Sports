using Serilog;

namespace Libs.RoundRobin.Mix;

public partial class Planner {

    private readonly ILogger log;

    public Planner() {
        log = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();
    }

    public void StartMixed(int men, int women, int games) {
        log.Information("Round Robin mix double: men {men}, women {women}, games {games}", men, women, games);
        CreateMix(men, women, games);
    }

}
