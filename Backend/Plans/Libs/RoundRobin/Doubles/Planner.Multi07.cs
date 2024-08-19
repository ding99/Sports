//using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    #region 7 games

    public void Select087() {
        int persons = 8, games = 7, times = 30, max = 1;
        Chose07(persons, games, times, max);
    }

    #endregion


    #region util

    public void Chose07(int persons, int games, int loop, int maxOppo) {
        Chose(persons, games, loop, persons, maxOppo, 2);
    }

    //public void Chose07_o(int persons, int games, int loop, int maxOppo) {
    //    int cn = 0;
    //    log.Information("Round Robin double: players {p}, games {games}. times {times}, maxC2 {max}", persons, games, loop, maxOppo);
    //    for (int i = 0; i < loop; i++) {
    //        if (i > 0 && i % 1000 == 0) {
    //            log.Information("-- loop {i}", i);
    //        }
    //        var result = CreateDbl(persons, games, false, false);

    //        if (result.IsSuccess) {
    //            var ps = result.Value.p;
    //            var o3 = Oppo3_07(ps);
    //            var p2 = Part2_07(ps);
    //            if (o3 < maxOppo && p2 <= persons) {
    //                log.Information("Master: {m}", result.Value.mst);
    //                log.Information("{i,2}: O3 {o3} P2 {p2}", ++cn, o3, p2);
    //                log.Information("{d}", DTour(result.Value.t, $"{persons}-Player {games}-Game"));
    //                log.Information("{d}", DPlayers(result.Value.p));
    //            }
    //        }
    //    }
    //    log.Information("Sum {cn}", cn);
    //}

    //public static int Oppo3_07(Player[] ps) {
    //    return ps.Sum(p => p.Opponents.Count(o => o > 2));
    //}

    //public static int Part2_07(Player[] ps) {
    //    return ps.Sum(p => p.Partners.Count(o => o > 1));
    //}

    #endregion

}
