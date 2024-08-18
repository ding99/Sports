using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {


    #region 6 games

    public void Select066() {
        int persons = 6, games = 6, times = 30, max = 13;
        Chose06(persons, games, times, max);
    }

    #endregion

    #region util

    public void Chose06(int persons, int games, int loop, int max) {
        int cn = 0;
        log.Information("Round Robin double: players {p}, games {games}. times {times}, maxC2 {max}", persons, games, loop, max);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var o3 = Oppo3_06(ps);
                var p2 = Part2_06(ps);
                if (o3 < max && p2 <= persons) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: O3 {o3} P2 {p2}", ++cn, o3, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-Player {games}-Game"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {cn}", cn);
    }

    public static int Oppo3_06(Player[] ps) {
        return ps.Sum(p => p.Opponents.Count(o => o > 2));
    }

    public static int Part2_06(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }

    #endregion

}
