using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    #region loop 9-4

    public void Chose94(int persons, int games, int loop) {
        int cn = 0, maxC2 = 10;
        log.Information("loop {l}, maxC2 {max}", loop, maxC2);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- i {i}", i);
            }
            var result = CreateDbl(persons, games, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3_94(ps);
                var c2 = Count2_94(ps);
                var p2 = Part2_94(ps);
                if (c3 < 1 && c2 < maxC2 && p2 < 1) {
                    cn++;
                    log.Information("-- {i,2}: C3 {c3} C2 {c2} P2 {p2}", cn, c3, c2, p2);
                    //log.Information("{i,2}: P2 {p2}", cn, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #endregion

    #region util 9-4
    public int Count3_94(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 2) + p.Opponents.Count(o => o > 2));
    }

    public int Count2_94(Player[] ps) {
        return ps.Sum(p => p.Opponents.Count(o => o > 2));
    }

    public int Part2_94(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }
    #endregion

}
