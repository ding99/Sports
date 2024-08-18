using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {
    public void Chose(int persons, int games, int loop, int maxOppo) {
        int cn = 0;
        log.Information("Round Robin double: players {p}, games {games}. times {times}, maxC2 {max}", persons, games, loop, maxOppo);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var o2 = OppoCount(ps);
                var p2 = PartCount(ps);
                if (o2 < maxOppo && p2 < 1) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: Oppo {o2} Part {p2}", ++cn, o2, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-Player {games}-Game"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {cn}", cn);
    }

    public static int OppoCount(Player[] ps, int min = 1) {
        return ps.Sum(p => p.Opponents.Count(o => o > min));
    }

    public static int PartCount(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }


}