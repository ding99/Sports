//using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    #region 4 games

    public void Select054() {
        int persons = 5, games = 4, times = 5, max = 21;
        Chose04(persons, games, times, max);
    }

    public void Select064() {
        int persons = 6, games = 4, times = 30, max = 19;
        Chose04(persons, games, times, max);
    }

    public void Select074() {
        int persons = 7, games = 4, times = 30, max = 15;
        Chose04(persons, games, times, max);
    }

    public void Select084() {
        int persons = 8, games = 4, times = 30, max = 9;
        Chose04(persons, games, times, max);
    }

    public void Select094() {
        int persons = 9, games = 4, times = 100, max = 11;
        Chose04(persons, games, times, max);
    }

    public void Select104() {
        int persons = 10, games = 4, times = 600, max = 9;
        Chose04(persons, games, times, max);
    }

    #endregion

    public void Chose04(int persons, int games, int loop, int maxOppo) {
        Chose(persons, games, loop, 1, maxOppo, 1);
        //int cn = 0;
        //log.Information("Round Robin double: players {p}, games {games}. times {times}, maxC2 {max}", persons, games, loop, maxOppo);
        //for (int i = 0; i < loop; i++) {
        //    if (i > 0 && i % 1000 == 0) {
        //        log.Information("-- loop {i}", i);
        //    }
        //    var result = CreateDbl(persons, games, false, false);

        //    if (result.IsSuccess) {
        //        var ps = result.Value.p;
        //        var o3 = Oppo3_06(ps);
        //        var p2 = Part2_06(ps);
        //        if (o3 < maxOppo && p2 <= persons) {
        //            log.Information("Master: {m}", result.Value.mst);
        //            log.Information("{i,2}: O3 {o3} P2 {p2}", ++cn, o3, p2);
        //            log.Information("{d}", DTour(result.Value.t, $"{persons}-Player {games}-Game"));
        //            log.Information("{d}", DPlayers(result.Value.p));
        //        }
        //    }
        //}
        //log.Information("Sum {cn}", cn);
    }

}
