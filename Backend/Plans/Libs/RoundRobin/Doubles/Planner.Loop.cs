using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    #region loop 8-4

    public void Chose8_4(int persons, int games, int loop) {
        int cn = 0, maxC2 = 9;
        log.Information("loop {l}, maxC2 {max}", loop, maxC2);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3_84(ps);
                var o2 = Oppo2_84(ps);
                var p2 = Part2_84(ps);
                if (c3 < 1 && o2 < maxC2 && p2 < 1) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: C3 {c3} O2 {o2} P2 {p2}", ++cn, c3, o2, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #region util 9-4
    public int Count3_84(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1) + p.Opponents.Count(o => o > 2));
    }

    public int Oppo2_84(Player[] ps) {
        return ps.Sum(p => p.Opponents.Count(o => o > 1));
    }

    public int Part2_84(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }

    #endregion

    #endregion

    #region loop 9-4

    public void Chose9_4(int persons, int games, int loop) {
        int cn = 0, maxC2 = 5;
        log.Information("loop {l}, maxC2 {max}", loop, maxC2);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3_9_4(ps);
                var o2 = Oppo2_9_4(ps);
                var p2 = Part2_9_4(ps);
                if (c3 < 1 && o2 < maxC2 && p2 < 1) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: C3 {c3} O2 {o2} P2 {p2}", ++cn, c3, o2, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #region util four
    public int Count3_9_4(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1) + p.Opponents.Count(o => o > 2));
    }

    public int Oppo2_9_4(Player[] ps) {
        return ps.Sum(p => p.Opponents.Count(o => o > 2));
    }

    public int Part2_9_4(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }

    #endregion

    #endregion

    #region loop 10-4

    public void Chose10_4(int persons, int games, int loop) {
        int cn = 0, maxC2 = 9;
        log.Information("loop {l}, maxC2 {max}", loop, maxC2);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3_10_4(ps);
                var o2 = Oppo2_10_4(ps);
                var p2 = Part2_10_4(ps);
                if (c3 < 1 && o2 < maxC2 && p2 < 1) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: C3 {c3} O2 {o2} P2 {p2}", ++cn, c3, o2, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #region util four
    public int Count3_10_4(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1) + p.Opponents.Count(o => o > 2));
    }

    public int Oppo2_10_4(Player[] ps) {
        return ps.Sum(p => p.Opponents.Count(o => o > 1));
    }

    public int Part2_10_4(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }

    #endregion

    #endregion

}
