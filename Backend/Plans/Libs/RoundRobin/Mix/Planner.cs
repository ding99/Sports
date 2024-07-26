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
        
        CreateMix(men, women, games, true);
    }

    public void Select66() {
        int men = 6, women = 6, games = 6;
        log.Information("Round Robin mix double: men {men}, women {women}, games {games}", men, women, games);
        Chose66(men, women, games, 100);
    }

    public void Select65() {
        int men = 6, women = 5, games = 6;
        log.Information("Round Robin mix double: men {men}, women {women}, games {games}", men, women, games);
        Chose65(men, women, games, 100000);
    }

}
