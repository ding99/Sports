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

    public void Start(int persons, int games, bool sample, bool multi) {
        if (sample) {
            SampleDouble(persons, games);
        } else if (multi) {
            MultiDouble(persons, games);
        } else {
            SingleDouble(persons, games);
        }
    }

    #region sub entries

    public void SampleDouble(int persons, int games) {
        log.Information("Round Robin double sample: persons {persons}, games {games}", persons, games);

        CreateDbl(persons, games, true, true);
    }

    public void MultiDouble(int persons, int games) {
        switch (persons) {
        case 5:
            if (games == 4) {
                Select054();
                return;
            }
            break;
        case 6:
            switch(games){
            case 4:
                Select064();
                return;
            case 6:
                Select066();
                return;
            }
            break;
        case 7:
            if (games == 4) {
                Select074();
                return;
            }
            break;
        case 8:
            switch(games) {
            case 4:
                Select084();
                return;
            //case 7:
            //    Select087();
            //    return;
            }
            break;
        case 9:
            if (games == 4) {
                Select094();
                return;
            }
            break;
        case 10:
            if (games == 40) {
                Select104();
                return;
            }
            break;
        }

        log.Error("Not support {p}-player {g}-game case yet!", persons, games);
    }

    public void SingleDouble(int persons, int games) {
        log.Information("Round Robin doubles: persons {persons}, games {games}", persons, games);

        CreateDbl(persons, games, true, false);
    }

    #endregion

}