using Serilog;

namespace Libs.RoundRobin.Mix;

public partial class Planner {

    private readonly Serilog.Core.Logger log;

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
        int men = 6, women = 6, games = 36;
        int times = 100;

        log.Information("Round Robin mix double: men {men}, women {women}, games {games}. times {times}", men, women, games, times);
        Chose66(men, women, games, times);
    }

    public void Select65() {
        int men = 6, women = 5, games = 30;
        int times = 500; // 200000;

        log.Information("Round Robin mix double: men {men}, women {women}, games {games}. times {times}", men, women, games, times);
        Chose65(men, women, games, times);
    }

}
